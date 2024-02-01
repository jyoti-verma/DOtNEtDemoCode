namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class ObservationDto : FhirResourceDto
{
	public string PatientIdentifier { get; set; } = null!;

	public int Category { get; set; }

	public CodingDto Code { get; set; } = null!;

	public DateTime EffectiveDateTime { get; set; }

	public string PerformerIdentifier { get; set; } = null!;

	public string Note { get; set; } = "";
}
