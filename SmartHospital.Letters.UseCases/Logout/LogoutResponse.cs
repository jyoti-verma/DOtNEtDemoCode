namespace SmartHospital.Letters.UseCases.Logout;

public sealed record LogoutResponse(
	string? Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse(Message, Code)
{
	public LogoutResponse(int code, string message)
		: this(message, code)
	{
	}
}
