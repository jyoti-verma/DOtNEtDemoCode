using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.UseCases.Search;

public sealed record SearchResponse(
	ICollection<SearchResult>? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<ICollection<SearchResult>>(Value, Message, Code)
{
	public SearchResponse(int code, string message) :
		this(null, message, code)
	{
	}
}
