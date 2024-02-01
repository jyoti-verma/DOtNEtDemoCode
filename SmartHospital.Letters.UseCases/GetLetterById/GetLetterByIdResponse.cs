namespace SmartHospital.Letters.UseCases.GetLetterById;

public sealed record GetLetterByIdResponse(
	Dtos.Letter? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<Dtos.Letter?>(Value, Message, Code)
{
	public GetLetterByIdResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
