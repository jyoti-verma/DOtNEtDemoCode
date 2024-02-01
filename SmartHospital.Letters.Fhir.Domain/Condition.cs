namespace SmartHospital.Letters.Fhir.Domain;

/// <summary>
///     A clinical condition, problem, diagnosis, or other event, situation, issue, or clinical concept that has risen to a
///     level of concern.
/// </summary>
public class Condition : FhirResource
{
	/// <summary>
	///     problem-list-item | encounter-diagnosis
	/// </summary>
	public ICollection<Coding> Categories { get; set; } = new List<Coding>();

	/// <summary>
	///     Identification of the condition, problem or diagnosis
	/// </summary>
	public ICollection<Coding> Codes { get; set; } = new List<Coding>();

	public Enums.ClinicalStatus ClinicalStatus { get; set; }
	public Enums.VerificationStatus VerificationStatus { get; set; }
	public FhirResource Patient { get; set; } = null!;
	public Period RecordedDate { get; set; } = null!;
	public string Summary { get; set; } = "";
	public FhirResource Observation { get; set; } = null!;
	public List<Observation> Observations { get; set; } = null!;

}
