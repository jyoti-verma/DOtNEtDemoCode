using System.Globalization;
using Hl7.Fhir.Rest;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirPractitionerDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;
	public FhirPractitionerDataExtraction(ExternalFhirClient externalFhirClient)
	{
		_externalFhirClient = externalFhirClient;
	}
	public async Task<List<Practitioner>> ExtractPractitionerData(SearchParams searchParams)
	{
		List<Hl7.Fhir.Model.Practitioner> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Practitioner>(searchParams));

		List<Practitioner> practitioners = new List<Practitioner>();
		foreach (Hl7.Fhir.Model.Practitioner practitioner in result)
		{
			Practitioner pr1 = new Practitioner();
			pr1.Identifier = practitioner.Id.ToString();
			foreach (Hl7.Fhir.Model.HumanName name in practitioner.Name)
			{
				pr1.HumanNames = new List<HumanName>
				{
					new()
					{
						GivenName = name.GivenElement.Count > 0 ? name.GivenElement.First().Value.ToString() : "",
						FamilyName = name.Family != null ? name.Family!.ToString() : "",
						Period = new Period { Start = name.Period!=null? DateTime.Parse(name.Period.Start.ToString(), CultureInfo.InvariantCulture):DateTime.MinValue ,
								End=name.Period!=null? DateTime.Parse(name.Period.End.ToString()!, CultureInfo.InvariantCulture):DateTime.MinValue},
						Name =  name.ToString(),
						Prefix = name.PrefixElement.Count > 0 ? name.PrefixElement.First().Value.ToString() : ""
					}
				};
			}
			foreach (Hl7.Fhir.Model.Address address in practitioner.Address)
			{
				pr1.Addresses = new List<Address>
				{
					new()
					{
						Period = new Period { Start = DateTime.Parse(address.Period!=null?address.Period.Start.ToString()!:DateTime.MinValue.ToString(), CultureInfo.InvariantCulture),
								End=DateTime.Parse(address.Period!=null?address.Period.End.ToString()!:DateTime.MinValue.ToString(), CultureInfo.InvariantCulture)},
						Text = Convert.ToString(address),
						Lines = new List<string> { address.Line.First()!=null?address.Line.First():""},
						City = address.City != null ? address.City.ToString() : "",
						Country = address.Country != null ? address.ToString() : "",
						PostalCode = address.PostalCode != null ? address.ToString() : "",
						State = address.State != null ? address.StateElement.First().Value.ToString() : ""
					}
				};
			}

			pr1.Organization = new Organization() { Identifier = "" };//not available in server
			practitioners.Add(pr1);

		}

		return practitioners;
	}

}
