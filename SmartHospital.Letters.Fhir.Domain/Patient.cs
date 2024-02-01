
namespace SmartHospital.Letters.Fhir.Domain;

public class Patient : Person
{
	public DateTime DateOfBirth { get; set; }
	public string CityOfBirth { get; set; } = "";
	public Enums.Genders Gender { get; set; }
	public bool Deceased { get; set; }
	public DateTime? DeceasedDateTime { get; set; }
}
