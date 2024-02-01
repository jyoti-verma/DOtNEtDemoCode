namespace SmartHospital.Letters.Api.Services;

internal sealed class LetterNotFoundException : Exception
{
	public LetterNotFoundException(Guid letterId)
		: base($"Letter with id {letterId} not found.")
	{
	}
}
