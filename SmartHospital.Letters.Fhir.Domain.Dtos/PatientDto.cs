namespace SmartHospital.Letters.Fhir.Domain.Dtos;

public class PatientDto : PersonDto
{
	public DateTime DateOfBirth { get; set; }
	public string CityOfBirth { get; set; } = "";
	public int Gender { get; set; }
	public bool Deceased { get; set; }
	public DateTime? DeceasedDateTime { get; set; }
}
