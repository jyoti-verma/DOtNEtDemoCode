using System.Globalization;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;
using static SmartHospital.Letters.Fhir.Domain.Enums;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
public class FhirObservationDataExtraction
{
	private readonly ExternalFhirClient _externalFhirClient;
	private readonly FhirPatientDataExtraction _fhirPatientDataExtraction;
	private readonly FhirPractitionerDataExtraction _fhirPractitionerDataExtraction;

	public FhirObservationDataExtraction(ExternalFhirClient externalFhirClient, FhirPatientDataExtraction fhirPatientDataExtraction, FhirPractitionerDataExtraction fhirPractitionerDataExtraction)
	{
		_externalFhirClient = externalFhirClient;
		_fhirPatientDataExtraction = fhirPatientDataExtraction;
		_fhirPractitionerDataExtraction = fhirPractitionerDataExtraction;
	}
	public string ExtractPatientIdFromObservation(string patientFhirDataString)
	{
		string patientId = string.Empty;
		JObject jsonObject = JObject.Parse(patientFhirDataString);

		if (jsonObject["entry"] is JArray entryArray && entryArray.Count > 0)
		{
			JObject resourceObject = entryArray[0]["resource"] as JObject;

			if (resourceObject != null && resourceObject["subject"] is JObject subjectObject)
			{
				string reference = (string)subjectObject["reference"];
				patientId = reference.Split('/')[1].ToString();
			}
		}
		return patientId;
	}

	public List<Observation> ExtractObservationData(string patientFhirDataString, ref int ErrorFlag)
	{
		var observationDtos = new List<Observation>();

		var jsonObject = JObject.Parse(patientFhirDataString);
		var entryArray = (JArray)jsonObject["entry"];

		foreach (JObject entry in entryArray)
		{
			var observation = new Observation
			{
				Identifier = "137256485",
				Category = Enums.Categories.Ambulance,
				EffectiveDateTime = DateTime.Parse("08/11/2015", CultureInfo.InvariantCulture),
				//Patient = Patients.Single(p => p.Identifier == "1679314678"),
				//Performer = Practitioners.Single(p => p.Identifier == "4863786296"),
				Code = new Coding { System = "med", Code = "1245.1", Display = "Erstbegutachtung" },
				Note = "Patientin klagt über Schmerzen beim Einatmen."
			};

			//observationDtos.Add();
		}

		return observationDtos;
	}

	public async Task<List<Observation>> ExtractObservationData2(SearchParams searchParams)
	{

		List<Hl7.Fhir.Model.Observation> result = await _externalFhirClient.ToListAsync(_externalFhirClient.SearchFhirAsync<Hl7.Fhir.Model.Observation>(searchParams));

		List<Observation> observations = new List<Observation>();
		foreach (Hl7.Fhir.Model.Observation observation in result)
		{
			Observation obs = new Observation();
			obs.Patient = new Patient { Identifier = observation.Subject != null ? observation.Subject.Url.OriginalString.Split("Patient/")[1] : "" };
			obs.Identifier = observation.Id.ToString();
			if (observation.Category.Count > 0)
			{
				obs.Category = Enum.TryParse(observation.Category.First().Coding.First().Code, true, out Categories categories) ?
						categories : Categories.Unknown;
			}

			obs.EffectiveDateTime = DateTime.Parse(observation.Effective != null ?
				observation.Effective.First().Value.ToString()! :
				DateTime.MinValue.ToString(), CultureInfo.InvariantCulture);
			obs.Performer = new Practitioner { Identifier = observation.Performer.Count > 0 ? observation.Performer.First().Url.OriginalString.Split("Practitioner/")[1] : "" };

			if (observation.Code != null && observation.Code.Coding.Count > 0)
			{
				foreach (var code in observation.Code.Coding)
				{
					obs.Code = new Coding
					{
						System = code.System ?? "",
						Code = code.Code ?? "",
						Display = code.Display ?? ""
					};
				}

			}
			else
			{
				obs.Code = new Coding { System = "", Code = "", Display = "" };

			}
			obs.Note = observation.Note.Count > 0 ? observation.Note.First()!.Text : "";
			observations.Add(obs);
		}
		return observations;

	}

}
