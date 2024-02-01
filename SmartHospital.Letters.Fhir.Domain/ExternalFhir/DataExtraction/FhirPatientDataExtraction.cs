using System.Globalization;
using System.Linq;
using Bogus.DataSets;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirPatientDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;
	public FhirPatientDataExtraction(ExternalFhirClient externalFhirClient)
	{
		_externalFhirClient = externalFhirClient;
	}
	public async Task<List<Patient>> GetPatientsBySerachParamsAsync(SearchParams searchParams)
	{
		List<Hl7.Fhir.Model.Patient> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Patient>(searchParams));
		List<Patient> patients = new List<Patient>();
		List<Hl7.Fhir.Model.Patient> patientByObservations = new List<Hl7.Fhir.Model.Patient>();

		if (searchParams.Query.Contains("observation"))
		{
			List<Hl7.Fhir.Model.Observation> observationResult = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Observation>(searchParams));
			foreach (Hl7.Fhir.Model.Observation observation in observationResult)
			{
				var patientId = new Patient { Identifier = observation.Subject != null ? observation.Subject.Url.OriginalString.Split("Patient/")[1] : "" };
				if (patientId!=null)
				{
					patientByObservations.Add(result.FirstOrDefault(x => x.Id == patientId.ToString()));
				}
			}
			result = patientByObservations;
		}
		foreach (Hl7.Fhir.Model.Patient patient in result)
		{
			Patient p1 = new Patient();

			if (patient.Name.Count > 0)
			{
				foreach (Hl7.Fhir.Model.HumanName name in patient.Name)
				{
					p1.HumanNames = new List<HumanName>
					{
						new()
						{
							GivenName = name.GetType().GetProperty("GivenElement")!=null? name.GivenElement.Count>0? name.GivenElement.First().Value.ToString() : "":"",
							FamilyName = name.GetType().GetProperty("Family")!=null? Convert.ToString(name.Family)!=null? name.Family.ToString() : "":"",
							Period = new Period {
								Start =name.GetType().GetProperty("Period")!=null?name.Period!=null?name.Period.Start!=null?DateTime.TryParse(name.Period.Start, out DateTime sDate)?
							DateTime.Parse(name.Period.Start.ToString(), CultureInfo.InvariantCulture)
							:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue,

							End=name.GetType().GetProperty("Period")!=null?name.Period!=null?name.Period.End!=null?DateTime.TryParse(name.Period.End, out DateTime eDate)?
							DateTime.Parse(name.Period.End.ToString(), CultureInfo.InvariantCulture)
							:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue
							},

							Prefix =  name.GetType().GetProperty("PrefixElement")!=null? name.PrefixElement.Count>0? name.PrefixElement.First().Value.ToString() : "":"",
						}
					};
				}
			}

			if (patient.Address.Count > 0)
			{

				foreach (Hl7.Fhir.Model.Address address in patient.Address)
				{
					p1.Addresses = new List<Address>
					{
						new()
						{
							Period = new Period {
								Start =address.GetType().GetProperty("Period")!=null?address.Period!=null?address.Period.Start!=null?DateTime.TryParse(address.Period.Start, out DateTime sDate)?
								DateTime.Parse(address.Period.Start.ToString(), CultureInfo.InvariantCulture)
								:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue,
								End=address.GetType().GetProperty("Period")!=null?address.Period!=null?address.Period.End!=null?DateTime.TryParse(address.Period.End, out DateTime eDate)?
								DateTime.Parse(address.Period.End.ToString(), CultureInfo.InvariantCulture)
							:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue:DateTime.MinValue
							},
							Text = address.GetType().GetProperty("Text")!=null?Convert.ToString(address):"",
							Lines = new List<string> { address.Line.Any()?address.Line.First() : ""},
							City = address.City != null ? address.City.ToString() : "",
							Country = address.Country != null ? address.ToString() ! : "",
							PostalCode = address.PostalCode != null ? address.ToString() ! : "",
							State = address.State != null ? address.StateElement.First().Value.ToString()!: ""
						}
					};
				}
			}
			p1.Identifier = patient.Id.ToString();
			p1.CityOfBirth = "";
			p1.DateOfBirth = DateTime.Parse(patient.BirthDateElement != null ? patient.BirthDate : "01/01/1990", CultureInfo.InvariantCulture);
			p1.Gender = patient.Gender != null ? (Enums.Genders)patient.Gender : Enums.Genders.Unknown;
			if (patient.Deceased != null)
			{
				bool isValidDate = DateTime.TryParse(patient.Deceased.First().Value.ToString(), out _);
				if (!isValidDate)
				{
					p1.Deceased = (bool)patient.Deceased.First().Value;
				}
				else
				{
					p1.DeceasedDateTime = Convert.ToDateTime(patient.Deceased.First().Value);
				}
			}

			patients.Add(p1);
		}
		return patients;
	}

	public List<Patient> ExtractpatientData(string? patientId, string patientFhirDataString, ref int ErrorFlag)
	{
		var patientDtos = new List<Patient>();

		var jsonObject = JObject.Parse(patientFhirDataString);
		var entryArray = (JArray)jsonObject["entry"];

		foreach (JObject entry in entryArray)
		{
			var resourceObject = (JObject)entry["resource"];
			var resourceType = (string)resourceObject["resourceType"];

			if (!resourceType.Contains("Patient"))
			{
				continue;
			}
			else
			{
				var addressArray = (JArray)resourceObject?["address"];
				var nullableDateTime = (DateTime?)resourceObject["birthDate"];
				var patient = new Patient
				{
					Identifier = (string)resourceObject["id"],
					DateOfBirth = nullableDateTime ?? DateTime.MinValue,
					Gender = GetGender((string)resourceObject["gender"]),
					Deceased = false,
					DeceasedDateTime = DateTime.MinValue,
					CityOfBirth = GetCityOfBirth(resourceObject)
				};

				List<HumanName> humanNameslist = GetHumanNames(resourceObject);
				patient.HumanNames = humanNameslist;
				var addresseslist = new List<Address>();
				if (addressArray != null && addressArray.Count() > 0)
				{
					addresseslist = GetAddresses(addressArray);
				}
				patient.Addresses = addresseslist;

				patientDtos.Add(patient);
			}

		}

		return patientDtos;
	}

	static Enums.Genders GetGender(string gender)
	{
		return gender switch
		{
			"male" => Enums.Genders.Male,
			"female" => Enums.Genders.Female,
			_ => Enums.Genders.Other
		};
	}

	string GetCityOfBirth(JObject resourceObject)
	{
		var addressArray = (JArray)resourceObject?["address"];
		if (addressArray != null && addressArray.Count() > 0)
		{
			return (string)addressArray[0]?["city"];
		}
		else
		{
			return "";
		}
	}

	List<HumanName> GetHumanNames(JObject resourceObject)
	{
		var nameArray = (JArray)resourceObject?["name"];

		return new List<HumanName>
	{
		new HumanName
		{
			Name = nameArray != null? (string)nameArray[0]?["family"] + " " + (string)nameArray[0]?["given"]?[0]: string.Empty,
			GivenName = nameArray != null? (string)nameArray[0]?["given"]?[0]: string.Empty,
			FamilyName = nameArray != null? (string)nameArray[0]?["family"] : string.Empty,
			Prefix = "",
			Suffix = ""
		}
	};
	}

	List<Address> GetAddresses(JArray addressArray)
	{
		if (addressArray != null && addressArray.Count > 0)
		{
			var addressObject = (JObject)addressArray[0];
			JArray lineArray = addressObject.ContainsKey("line") ? (JArray)addressObject["line"] : new JArray();
			return new List<Address>
		{
			new Address
			{
				Text = (string)lineArray?.FirstOrDefault(),
				Lines = lineArray?.Select(item => (string)item).ToList(),
				City = addressObject.ContainsKey("city") ? (string)addressObject["city"] : string.Empty,
				District = addressObject.ContainsKey("district") ? (string)addressObject["district"] : string.Empty,
				State = addressObject.ContainsKey("state") ? (string)addressObject["state"] : string.Empty,
				PostalCode = addressObject.ContainsKey("postalCode") ? (string)addressObject["postalCode"] : string.Empty,
				Country = addressObject.ContainsKey("country") ? (string)addressObject["country"] : string.Empty
			}
		};
		}
		return new List<Address>();
	}
}
