namespace SmartHospital.Letters.Dtos;

public sealed record TokenResponse(
	string AccessToken,
	string RefreshToken
);
