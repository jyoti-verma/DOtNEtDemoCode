namespace SmartHospital.Letters.Fhir.Domain;

public abstract class Person : FhirResource
{
	public ICollection<HumanName> HumanNames { get; set; } = new List<HumanName>();
	public ICollection<Address> Addresses { get; set; } = new List<Address>();
}
