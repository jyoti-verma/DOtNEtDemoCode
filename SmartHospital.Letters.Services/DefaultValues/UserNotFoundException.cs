namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class UserNotFoundException : Exception
{
	public UserNotFoundException(string user)
		: base($"User {user} not found")
	{
		Data.Add("user", user);
	}
}
