using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Core.Extensions;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Services;

public sealed class JwtService : IJwtService
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly LetterDbContext _dbContext;
	private readonly IOptionsMonitor<JwtServiceOptions> _options;


	public JwtService(
		IDateTimeProvider dateTimeProvider,
		LetterDbContext dbContext,
		IOptionsMonitor<JwtServiceOptions> options
	)
	{
		_dateTimeProvider = dateTimeProvider;
		_dbContext = dbContext;
		_options = options;
	}

	public async Task<Token> GenerateTokenAsync(LetterUser letterUser, CancellationToken cancellationToken = default)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.Key));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Issuer = _options.CurrentValue.ValidIssuer,
			Subject = new ClaimsIdentity(await CreateClaimsAsync(letterUser, cancellationToken)),
			Expires = _dateTimeProvider.Now + _options.CurrentValue.TokenLifetime,
			SigningCredentials = credentials
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

		return new Token(
			tokenHandler.WriteToken(token),
			GenerateRefreshToken(),
			token.ValidFrom.Add(_options.CurrentValue.RefreshTokenLifetime)
		);
	}

	public string GenerateRefreshToken()
	{
		byte[] randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);

		return Convert.ToBase64String(randomNumber);
	}

	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false,
			ValidateIssuer = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.CurrentValue.Key)),
			ValidateLifetime = true,
			ValidAudience = _options.CurrentValue.ValidAudience,
			ValidIssuer = _options.CurrentValue.ValidIssuer
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		ClaimsPrincipal? principal =
			tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
		return securityToken is not JwtSecurityToken jwtSecurityToken ||
		       !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
			       StringComparison.InvariantCultureIgnoreCase)
			? throw new SecurityTokenException("Invalid token")
			: principal;
	}

	private Task<List<Claim>> CreateClaimsAsync(
		LetterUser letterUser,
		CancellationToken cancellationToken = default
	)
	{
		var userRoles = _dbContext.UserRoles
			.Where(e => e.UserId == letterUser.Id)
			.ToList();
		IEnumerable<string> roleIds = userRoles.Select(r => r.RoleId);
		var roles = _dbContext.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
		var userClaims =
			_dbContext.UserClaims.Where(uc => uc.UserId == letterUser.Id).ToList();
		var roleClaims =
			_dbContext.RoleClaims.Where(rc => roleIds.Contains(rc.RoleId)).ToList();

		var claims = new List<Claim>
		{
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			new(ClaimTypes.NameIdentifier, letterUser.Id),
			new(JwtRegisteredClaimNames.Sub, letterUser.Email ?? ""),
			new(nameof(letterUser.Id).ToCamelCase(), letterUser.Id),
			new(nameof(letterUser.Firstname).ToCamelCase(), letterUser.Firstname),
			new(nameof(letterUser.Lastname).ToCamelCase(), letterUser.Lastname),
			new(nameof(letterUser.Title).ToCamelCase(), letterUser.Title),
			new(nameof(letterUser.Salutation).ToCamelCase(), letterUser.Salutation),
			new(nameof(letterUser.UserName).ToCamelCase(), letterUser.UserName!)
		};

		AddUserRoles(cancellationToken, roles, claims);
		AddUserClaims(cancellationToken, userClaims, claims);
		AddIdentityRoleClaim(cancellationToken, roleClaims, claims);

		return Task.FromResult(claims);
	}

	private static void AddIdentityRoleClaim(CancellationToken cancellationToken,
		List<IdentityRoleClaim<string>> roleClaims, List<Claim> claims)
	{
		foreach (IdentityRoleClaim<string> roleClaim in roleClaims)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			if (roleClaim.ClaimValue is null)
			{
				continue;
			}

			if (roleClaim.ClaimType is null)
			{
				continue;
			}

			claims.Add(new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
		}
	}

	private static void AddUserRoles(CancellationToken cancellationToken, List<IdentityRole> roles, List<Claim> claims)
	{
		foreach (IdentityRole role in roles)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			if (role.Name is null)
			{
				continue;
			}

			claims.Add(new Claim(ClaimTypes.Role, role.Name));
		}
	}

	private static void AddUserClaims(CancellationToken cancellationToken, List<IdentityUserClaim<string>> userClaims,
		List<Claim> claims)
	{
		foreach (IdentityUserClaim<string> userClaim in userClaims)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			if (userClaim.ClaimValue is null)
			{
				continue;
			}

			if (userClaim.ClaimType is null)
			{
				continue;
			}

			claims.Add(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
		}
	}
}
