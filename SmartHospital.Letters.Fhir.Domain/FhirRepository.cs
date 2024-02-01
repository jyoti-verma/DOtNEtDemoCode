using System.Xml.Linq;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;

namespace SmartHospital.Letters.Fhir.Domain;

public class FhirRepository : IFhirRepository
{
	public ICollection<Patient> Patients { get; }
		= new MockPatientsCollection()
			.ToList();

	public ICollection<Organization> Organizations { get; }
		= new MockOrganizationsCollection()
			.ToList();

	public ICollection<Practitioner> Practitioners { get; }
		= new MockPractitionersCollection()
			.ToList();

	public ICollection<Observation> Observations { get; }
		= new MockObservationsCollection()
			.ToList();

	public ICollection<Condition> Conditions { get; }
		= new MockConditionsCollection()
			.ToList();

	public ICollection<DiagnosticReport> DiagnosticReports { get; }
		= new MockDiagnosticReportCollection()
			.ToList();

}
