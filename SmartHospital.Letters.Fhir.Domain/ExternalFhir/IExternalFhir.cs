namespace SmartHospital.Letters.Fhir.Domain.ExternalFhir;
internal interface IExternalFhir
{
	ICollection<Patient> GetPatient(string? patientId, string? patientName);

}
