namespace SmartHospital.Letters.Services;

public class JwtServiceOptions
{
	public string Key { get; set; } = "";
	public string ValidAudience { get; set; } = "";
	public string ValidIssuer { get; set; } = "";
	public TimeSpan TokenLifetime { get; set; }
	public TimeSpan RefreshTokenLifetime { get; set; }
	public static string Section { get; set; } = "Jwt";
}
