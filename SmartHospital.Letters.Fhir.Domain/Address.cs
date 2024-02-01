namespace SmartHospital.Letters.Fhir.Domain;

public class Address
{
	public string Text { get; set; } = "";
	public ICollection<string> Lines { get; set; } = new List<string>();
	public string City { get; set; } = "";
	public string District { get; set; } = "";
	public string State { get; set; } = "";
	public string PostalCode { get; set; } = "";
	public string Country { get; set; } = "";
	public Period? Period { get; set; }
}
