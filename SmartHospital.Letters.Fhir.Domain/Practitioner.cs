namespace SmartHospital.Letters.Fhir.Domain;

public class Practitioner : Person
{
	public Organization Organization { get; set; } = null!;
}
