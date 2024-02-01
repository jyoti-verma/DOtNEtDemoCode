using SmartHospital.Letters.Fhir.Domain.ExternalFhir;

namespace SmartHospital.Letters.Fhir.Domain;

public interface IFhirRepository: IGlobalRepo
{
	ICollection<Patient> Patients { get; }
	ICollection<Observation> Observations { get; }
	ICollection<Organization> Organizations { get; }
	ICollection<Practitioner> Practitioners { get; }
	ICollection<Condition> Conditions { get; }
	ICollection<DiagnosticReport> DiagnosticReports { get; }
}
