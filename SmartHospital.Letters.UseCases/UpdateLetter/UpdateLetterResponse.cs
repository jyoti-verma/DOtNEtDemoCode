namespace SmartHospital.Letters.UseCases.UpdateLetter;

public sealed record UpdateLetterResponse(
	Dtos.Letter? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<Dtos.Letter?>(Value, Message, Code)
{
	public UpdateLetterResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
