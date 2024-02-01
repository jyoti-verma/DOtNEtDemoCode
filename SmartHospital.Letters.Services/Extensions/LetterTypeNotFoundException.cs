namespace SmartHospital.Letters.Services.Extensions;

public sealed class LetterTypeNotFoundException : Exception
{
	public LetterTypeNotFoundException(string name)
		: base($"Letter type with name {name} not found")
	{
	}
}
