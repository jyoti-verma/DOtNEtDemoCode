namespace SmartHospital.Letters.Fhir.Domain;

public class HumanName
{
	public string Name { get; set; } = "";
	public string GivenName { get; set; } = "";
	public string FamilyName { get; set; } = "";
	public string Prefix { get; set; } = "";
	public string Suffix { get; set; } = "";
	public Period? Period { get; set; }
}
