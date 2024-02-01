namespace SmartHospital.Letters.Domain;

public record Token(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiration);
