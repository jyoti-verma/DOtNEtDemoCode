using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Api.Authentication;

/// <summary>
///     Validate JWT token
/// </summary>
internal sealed class BearerAuthenticationHandler : SignOutAuthenticationHandler<AuthenticationSchemeOptions>
{
	private readonly IOptions<JwtBearerOptions> _jwtBearerOptions;
	private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
	private readonly SignInManager<LetterUser> _signInManager;

	/// <summary>
	///     Validate JWT token
	/// </summary>
	/// <param name="options"></param>
	/// <param name="logger"></param>
	/// <param name="encoder"></param>
	/// <param name="clock"></param>
	/// <param name="jwtSecurityTokenHandler"></param>
	/// <param name="jwtBearerOptions"></param>
	/// <param name="signInManager"></param>
	public BearerAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		ISystemClock clock,
		JwtSecurityTokenHandler jwtSecurityTokenHandler,
		IOptions<JwtBearerOptions> jwtBearerOptions,
		SignInManager<LetterUser> signInManager
	) : base(options, logger, encoder, clock)
	{
		_jwtSecurityTokenHandler = jwtSecurityTokenHandler;
		_jwtBearerOptions = jwtBearerOptions;
		_signInManager = signInManager;
	}

	/// <summary>
	///     Validate JWT token
	/// </summary>
	/// <returns></returns>
	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.ContainsKey("Authorization"))
		{
			return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
		}

		string? token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
		if (token is null)
		{
			return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
		}

		try
		{
			return Task.FromResult(AuthenticateResult.Success(
				new AuthenticationTicket(
					_jwtSecurityTokenHandler.ValidateToken(
						token,
						_jwtBearerOptions.Value.TokenValidationParameters,
						out SecurityToken? _
					),
					Scheme.Name
				)
			));
		}
		catch (Exception e)
		{
			return Task.FromResult(AuthenticateResult.Fail(e.Message));
		}
	}

	protected override async Task HandleSignOutAsync(AuthenticationProperties? properties)
	{
		await _signInManager.SignOutAsync();
	}
}
