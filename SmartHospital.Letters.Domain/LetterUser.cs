using Microsoft.AspNetCore.Identity;

namespace SmartHospital.Letters.Domain;

public class LetterUser : IdentityUser
{
	[PersonalData] public string Firstname { get; set; } = "";

	[PersonalData] public string Lastname { get; set; } = "";

	[PersonalData] public string Title { get; set; } = "";

	[PersonalData] public string? RefreshToken { get; set; }

	[PersonalData] public DateTime? RefreshTokenExpiryTime { get; set; }

	[PersonalData] public string Salutation { get; set; } = "";
}
