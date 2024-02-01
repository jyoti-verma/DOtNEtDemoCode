namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class CodingDto
{
	public string System { get; set; } = "";
	public string Code { get; set; } = null!;
	public string Display { get; set; } = null!;
}
