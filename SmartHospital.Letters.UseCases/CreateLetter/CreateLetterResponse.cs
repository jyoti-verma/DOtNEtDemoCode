namespace SmartHospital.Letters.UseCases.CreateLetter;

public sealed record CreateLetterResponse(
	Dtos.Letter? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<Dtos.Letter>(Value, Message, Code)
{
	public CreateLetterResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
