using static SmartHospital.Letters.Fhir.Domain.Enums;

namespace SmartHospital.Letters.Fhir.Domain;

public class DiagnosticReport : FhirResource
{
	public Patient Patient { get; set; } = null!;
	public Observation Observation { get; set; } = null!;
	public DateTime EffectiveDateTime { get; set; }
	public Organization Performer { get; set; } = null!;
	public DateTime CollectedDateTime { get; set; }
	public string HeaderDiagnosis { get; set; } = "";

	public SpecimenMethod SpecimenMethod { get; set; } = SpecimenMethod.Unknown;

	// ICD-10
	public Coding TumorEntity { get; set; } = null!;

	// ICD-O
	public Coding TumorMorphology { get; set; } = null!;

	// Histologie
	public Coding TumorHistology { get; set; } = null!;

	// Molekularpathologie
	public ICollection<Coding> MolecularPathologyFindings { get; set; } = new List<Coding>();

	// Tumorstadium
	public ICollection<Coding> TumorStadium { get; set; } = new List<Coding>();

	// ECOG
	public int EcogPerformanceStatus { get; set; }
}
