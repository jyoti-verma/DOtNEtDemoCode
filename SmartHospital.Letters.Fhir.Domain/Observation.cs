using static SmartHospital.Letters.Fhir.Domain.Enums;

namespace SmartHospital.Letters.Fhir.Domain;

public class Observation : FhirResource
{
	public Patient Patient { get; set; } = null!;

	public Categories Category { get; set; }

	public Coding Code { get; set; } = null!;

	public DateTime EffectiveDateTime { get; set; }

	public Practitioner? Performer { get; set; }

	public string Note { get; set; } = "";
}
