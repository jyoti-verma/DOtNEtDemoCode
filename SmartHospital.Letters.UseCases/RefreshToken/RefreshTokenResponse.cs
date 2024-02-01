using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.UseCases.RefreshToken;

public sealed record RefreshTokenResponse(
	TokenResponse? Value,
	string Message = "Success",
	int Code = (int)Codes.Success
) : BaseResponse<TokenResponse?>(Value, Message, Code)
{
	public RefreshTokenResponse(int code, string message)
		: this(null, message, code)
	{
	}
}
