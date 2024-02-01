namespace SmartHospital.Letters.Fhir.Domain.Dtos;

/// <summary>
///     A clinical condition, problem, diagnosis, or other event, situation, issue, or clinical concept that has risen to a
///     level of concern.
/// </summary>
public class ConditionDto : FhirResourceDto
{
	/// <summary>
	///     problem-list-item | encounter-diagnosis
	/// </summary>
	public ICollection<CodingDto> Categories { get; set; } = new List<CodingDto>();

	/// <summary>
	///     Identification of the condition, problem or diagnosis
	/// </summary>
	public ICollection<CodingDto> Codes { get; set; } = new List<CodingDto>();

	public int ClinicalStatus { get; set; }
	public int VerificationStatus { get; set; }
	public string PatientIdentifier { get; set; } = null!;
	public PeriodDto RecordedDate { get; set; } = null!;
	public string Summary { get; set; } = "";
	public string ObservationIdentifier { get; set; } = null!;
}
