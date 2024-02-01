namespace SmartHospital.Letters.UseCases.GetLetters;

public sealed record GetLettersResponse(
	ICollection<Dtos.Letter> Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<ICollection<Dtos.Letter>?>(Value, Message, Code)
{
	public GetLettersResponse(int code, string message)
		: this(new List<Dtos.Letter>(), message, code)
	{
	}
}
