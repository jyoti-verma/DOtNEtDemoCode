using System.Security.Claims;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Services;

public interface IJwtService
{
	Task<Token> GenerateTokenAsync(LetterUser letterUser, CancellationToken cancellationToken = default);
	string GenerateRefreshToken();
	ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
