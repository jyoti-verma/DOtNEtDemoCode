using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.UseCases.GetSnippets;

public sealed record GetSnippetsResponse(
	ICollection<Snippet>? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<ICollection<Snippet>?>(Value, Message, Code)
{
	public GetSnippetsResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
