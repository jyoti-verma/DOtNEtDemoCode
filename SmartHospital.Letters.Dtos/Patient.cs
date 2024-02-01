using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Dtos;

public sealed class Patient
{
	public Patient(PatientDto patientDto)
	{
		Identifier = patientDto.Identifier;
		DateOfBirth = patientDto.DateOfBirth;
		Gender = patientDto.Gender;

		HumanNameDto? humanName = patientDto.HumanNames.SingleOrDefault(p => p.Period is { End: null });
		if (humanName is null)
		{
			return;
		}

		Name = humanName.Name;
		GivenName = humanName.GivenName;
		FamilyName = humanName.FamilyName;
		Prefix = humanName.Prefix;
		Suffix = humanName.Suffix;
	}

	public string Identifier { get; init; }
	public string Name { get; init; } = "";
	public string GivenName { get; init; } = "";
	public string FamilyName { get; init; } = "";
	public string Prefix { get; init; } = "";
	public string Suffix { get; init; } = "";
	public DateTime DateOfBirth { get; init; }
	public int Gender { get; init; }
}
