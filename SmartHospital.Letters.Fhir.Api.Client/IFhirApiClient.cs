using System.ComponentModel.DataAnnotations;
using Refit;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Fhir.Api.Client;

public interface IFhirApiClient
{
	[Get("/Patient/{identifier}")]
	Task<PatientDto> GetPatient(
		[Required] string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Patient")]
	Task<IEnumerable<PatientDto>> GetPatients(
		string? name = null,
		string? observationIdentifier = null,
		CancellationToken cancellationToken = default
	);

	[Get("/Observation")]
	Task<IEnumerable<ObservationDto>> GetObservations(
		[Required] string? patientIdentifier = null,
		CancellationToken cancellationToken = default
	);

	[Get("/Observation/{identifier}")]
	Task<ObservationDto> GetObservation(
		[Required] string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Organization")]
	Task<IEnumerable<OrganizationDto>> GetOrganizations(
		string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Condition")]
	Task<ConditionDto> GetCondition(
		[Required] string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Condition/Patient/{patientIdentifier}")]
	Task<IEnumerable<ConditionDto>> GetConditionsByPatientIdentifier(
		[Required] string patientIdentifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Condition/Observation/{observationIdentifier}")]
	Task<IEnumerable<ConditionDto>> GetConditionsByObservationIdentifier(
		[Required] string observationIdentifier,
		CancellationToken cancellationToken = default
	);

	[Get("/Practitioner")]
	Task<IEnumerable<PractitionerDto>> GetPractitioners(
		string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/DiagnosticReport/{identifier}")]
	Task<IEnumerable<DiagnosticReportDto>> GetDiagnosticReport(
		string identifier,
		CancellationToken cancellationToken = default
	);

	[Get("/DiagnosticReport/ByPatientAndObservation")]
	Task<IEnumerable<DiagnosticReportDto>> GetDiagnosticReportByPatientAndObservation(
		[Required] string patientIdentifier,
		[Required] string observationIdentifier,
		CancellationToken cancellationToken = default
	);
}
