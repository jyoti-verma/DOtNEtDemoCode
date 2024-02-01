using Hl7.Fhir.Rest;

namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;
public interface IExternalFhirRepo : IGlobalRepo
{

	ICollection<Patient> GetPatient(string? patientId, string? patientName);
	Task<ICollection<Patient>> GetPatient2(SearchParams searchParams);
	Task<ICollection<Organization>> GetOrganization(SearchParams searchParams);
	Task<ICollection<Condition>> GetCondition(SearchParams searchParams);
	//ICollection<Condition> GetCondition(SearchParams searchParams, string resourceType);
	Task<ICollection<Observation>> GetObservation(SearchParams searchParams);
	Task<ICollection<Practitioner>> GetPractitioner(SearchParams searchParams);

	Task<ICollection<DiagnosticReport>> GetDiagnosticReport(SearchParams searchParams);

}
