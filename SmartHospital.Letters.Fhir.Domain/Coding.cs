namespace SmartHospital.Letters.Fhir.Domain;

public class Coding
{
	public string System { get; set; } = "";
	public string Code { get; set; } = null!;
	public string Display { get; set; } = null!;
}
