namespace SmartHospital.Letters.Fhir.Domain;

public class Organization : FhirResource
{
	public string Name { get; set; } = null!;
	public Coding Type { get; set; } = null!;
	public Address? Address { get; set; } = null!;
	public Organization? PartOf { get; set; }
}
