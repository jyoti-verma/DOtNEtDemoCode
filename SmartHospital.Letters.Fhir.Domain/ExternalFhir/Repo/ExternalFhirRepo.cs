using Hl7.Fhir.Rest;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;
public class ExternalFhirRepo : IExternalFhirRepo
{
	private readonly ExternalFhirClient _externalFhirClient;
	private readonly FhirObservationDataExtraction _fhirObservationDataExtraction;
	private readonly FhirPatientDataExtraction _fhirPatientDataExtraction;
	private readonly FhirOrganizationDataExtraction _fhirOrganizationDataExtraction;
	private readonly FhirConditionDataExtraction _fhirConditionDataExtraction;
	private readonly FhirDiagnosticReportDataExtraction _fhirDiagnosticReportDataExtraction;
	private readonly FhirPractitionerDataExtraction _fhirPractitionerDataExtraction;


	public ExternalFhirRepo(ExternalFhirClient externalFhirClient, FhirPatientDataExtraction fhirPatientDataExtraction,
		FhirObservationDataExtraction fhirObservationDataExtraction, FhirOrganizationDataExtraction fhirOrganizationDataExtraction,
		FhirConditionDataExtraction fhirConditionDataExtraction, FhirDiagnosticReportDataExtraction fhirDiagnosticReportDataExtraction,
		FhirPractitionerDataExtraction fhirPractitionerDataExtraction)
	{
		_externalFhirClient = externalFhirClient;
		_fhirPatientDataExtraction = fhirPatientDataExtraction;
		_fhirObservationDataExtraction = fhirObservationDataExtraction;
		_fhirOrganizationDataExtraction = fhirOrganizationDataExtraction;
		_fhirConditionDataExtraction = fhirConditionDataExtraction;
		_fhirDiagnosticReportDataExtraction = fhirDiagnosticReportDataExtraction;
		_fhirPractitionerDataExtraction = fhirPractitionerDataExtraction;

	}
	ICollection<Patient> IExternalFhirRepo.GetPatient(string? observationIdentifier, string? patientName)
	{
		int ErrorFlag = 0;
		List<Patient> patientDtos = new List<Patient>();
		string? fhirServerUrl;
		string? fhirData;
		//patientId = "592744";
		Enums.FHIRCallMode fHIRCallMode;
		if (patientName == null)
		{
			fHIRCallMode = Enums.FHIRCallMode.Patient;
			fhirServerUrl = _externalFhirClient.GenerateFhirServerUrl(observationIdentifier, patientName, fHIRCallMode);
			var data = _externalFhirClient.ReadDataFromJson();
			//fhirData = _externalFhirClient.GetFhirDataHttpClient(fhirServerUrl);
			patientDtos = _fhirPatientDataExtraction.ExtractpatientData(observationIdentifier, data, ref ErrorFlag);
		}
		else if (observationIdentifier != null)
		{
			fHIRCallMode = Enums.FHIRCallMode.Observation;
			fhirServerUrl = _externalFhirClient.GenerateFhirServerUrl(observationIdentifier, patientName, fHIRCallMode);
			fhirData = _externalFhirClient.GetFhirDataHttpClient(fhirServerUrl);
			string patientId = _fhirObservationDataExtraction.ExtractPatientIdFromObservation(fhirData);
			if (patientId != null)
			{
				observationIdentifier = patientId;
				patientName = string.Empty;
				fHIRCallMode = Enums.FHIRCallMode.Patient;
				fhirServerUrl = _externalFhirClient.GenerateFhirServerUrl(observationIdentifier, patientName, fHIRCallMode);
				fhirData = _externalFhirClient.GetFhirDataHttpClient(fhirServerUrl);
				patientDtos = _fhirPatientDataExtraction.ExtractpatientData(observationIdentifier, fhirData, ref ErrorFlag);
			}
		}

		return patientDtos;
	}
	async Task<ICollection<Patient>> IExternalFhirRepo.GetPatient2(SearchParams searchParams)
	{
		List<Patient> patients = await _fhirPatientDataExtraction.GetPatientsBySerachParamsAsync(searchParams);
		return patients;
	}
	async Task<ICollection<Organization>> IExternalFhirRepo.GetOrganization(SearchParams searchParams)
	{
		List<Organization> organizations = _fhirOrganizationDataExtraction.ExtractOrganizationData(searchParams).Result;
		return organizations;
	}

	async Task<ICollection<Observation>> IExternalFhirRepo.GetObservation(SearchParams searchParams)
	{

		List<Observation> observations = _fhirObservationDataExtraction.ExtractObservationData2(searchParams).Result;
		return observations;
	}
	async Task<ICollection<Condition>> IExternalFhirRepo.GetCondition(SearchParams searchParams)
	{
		List<Condition> conditions = _fhirConditionDataExtraction.ExtractConditionData(searchParams).Result;
		return conditions;
	}
	//ICollection<Condition> IExternalFhirRepo.GetCondition(SearchParams searchParams, string resourceType)
	//{
	//	List<Condition> conditions = _fhirConditionDataExtraction.ExtractConditionData(searchParams, resourceType);
	//	return conditions;
	//}
	async Task<ICollection<DiagnosticReport>> IExternalFhirRepo.GetDiagnosticReport(SearchParams searchParams)
	{

		List<DiagnosticReport> diagnosticReports = _fhirDiagnosticReportDataExtraction.ExtractDiagnosticReportData(searchParams).Result;
		return diagnosticReports;
	}
	async Task<ICollection<Practitioner>> IExternalFhirRepo.GetPractitioner(SearchParams searchParams)
	{
		List<Practitioner> practitioners = _fhirPractitionerDataExtraction.ExtractPractitionerData(searchParams).Result;
		return practitioners;
	}

}
