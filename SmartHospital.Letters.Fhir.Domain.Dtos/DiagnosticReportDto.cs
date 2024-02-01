namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class DiagnosticReportDto : FhirResourceDto
{
	public string PatientIdentifier { get; set; } = null!;
	public string ObservationIdentifier { get; set; } = null!;
	public DateTime EffectiveDateTime { get; set; }
	public string PerformerIdentifier { get; set; } = null!;
	public DateTime CollectedDateTime { get; set; }
	public string HeaderDiagnosis { get; set; } = "";

	public int SpecimenMethod { get; set; }

	// ICD-10
	public CodingDto TumorEntity { get; set; } = null!;

	// ICD-O
	public CodingDto TumorMorphology { get; set; } = null!;

	// Histologie
	public CodingDto TumorHistology { get; set; } = null!;

	// Molekularpathologie
	public ICollection<CodingDto> MolecularPathologyFindings { get; set; } = new List<CodingDto>();

	// Tumorstadium
	public ICollection<CodingDto> TumorStadium { get; set; } = new List<CodingDto>();

	// ECOG
	public int EcogPerformanceStatus { get; set; }
}
