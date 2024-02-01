namespace SmartHospital.Letters.UseCases.CreateLetterFromExisting;

public sealed record CreateLetterFromExistingResponse(
	Dtos.Letter? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<Dtos.Letter>(Value, Message, Code)
{
	public CreateLetterFromExistingResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
