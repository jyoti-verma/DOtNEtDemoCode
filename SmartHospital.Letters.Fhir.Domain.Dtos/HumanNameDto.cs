namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class HumanNameDto
{
	public string Name { get; set; } = "";
	public string GivenName { get; set; } = "";
	public string FamilyName { get; set; } = "";
	public string Prefix { get; set; } = "";
	public string Suffix { get; set; } = "";
	public PeriodDto? Period { get; set; }
}
