using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Services.DefaultValues;

public sealed class SystemUser : LetterUser
{
	public SystemUser()
	{
		UserName = Environment.UserName;
	}
}
