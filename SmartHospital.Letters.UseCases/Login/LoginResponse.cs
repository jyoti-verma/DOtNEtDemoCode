using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.UseCases.Login;

public sealed record LoginResponse(
	TokenResponse? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<TokenResponse?>(Value, Message, Code)
{
	public LoginResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
