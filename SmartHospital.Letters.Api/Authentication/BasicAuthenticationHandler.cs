using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SmartHospital.Letters.Core.Extensions;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Services;
using SmartHospital.Letters.UseCases;
using SmartHospital.Letters.UseCases.Extensions;
using SmartHospital.Letters.UseCases.Login;

namespace SmartHospital.Letters.Api.Authentication;

/// <summary>
///     Does basic authentication
/// </summary>
internal sealed class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	private readonly IJwtService _jwtService;
	private readonly ILogger<BasicAuthenticationHandler> _logger;
	private readonly IMediator _mediator;
	private readonly SignInManager<LetterUser> _signInManager;
	private readonly UserManager<LetterUser> _userManager;

	public BasicAuthenticationHandler(
		IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory loggerFactory,
		UrlEncoder encoder,
		ISystemClock clock,
		UserManager<LetterUser> userManager,
		IJwtService jwtService,
		SignInManager<LetterUser> signInManager,
		ILogger<BasicAuthenticationHandler> logger,
		IMediator mediator
	)
		: base(options, loggerFactory, encoder, clock)
	{
		_userManager = userManager;
		_jwtService = jwtService;
		_signInManager = signInManager;
		_logger = logger;
		_mediator = mediator;
	}


	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (!Request.Headers.ContainsKey("Authorization"))
		{
			Response.Headers.Add("WWW-Authenticate", "Basic");
			return AuthenticateResult.Fail("Missing Authorization Header");
		}

		try
		{
			var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
			if (authHeader.Parameter is null)
			{
				Response.Headers.Add("WWW-Authenticate", "Basic");
				return AuthenticateResult.Fail("Missing Authorization Header");
			}

			byte[] credentialBytes = Convert.FromBase64String(authHeader.Parameter);
			string[] credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
			string username = credentials[0];
			string password = credentials[1];

			LetterUser? user = await _userManager.FindByNameAsync(username);
			if (user is null)
			{
				_logger.LogDebug("User with {UserName} not found", username);
				return AuthenticateResult.Fail("Invalid username or password.");
			}

			SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					_logger.LogDebug("User {UserName} is locked out", username);
					return AuthenticateResult.Fail("Invalid username or password.");
				}

				if (result.IsNotAllowed)
				{
					_logger.LogDebug("User {UserName} is not authorized", username);
					return AuthenticateResult.Fail("Invalid username or password.");
				}

				if (result.RequiresTwoFactor)
				{
					_logger.LogDebug("User {UserName} requires two factor authentication", username);
					return AuthenticateResult.Fail("Invalid username or password.");
				}

				_logger.LogDebug("Could not login user {UserName}", username);
				return AuthenticateResult.Fail("Invalid username or password.");
			}


			Token token = await _jwtService.GenerateTokenAsync(user, CancellationToken.None);
			Claim[] claims =
			{
				new(ClaimTypes.NameIdentifier, user.Id),
				new(nameof(Token.AccessToken).ToCamelCase(), token.AccessToken),
				new(nameof(Token.RefreshToken).ToCamelCase(), token.RefreshToken),
				new(nameof(Token.RefreshTokenExpiration).ToCamelCase(), token.RefreshTokenExpiration.ToString("o"))
			};

			var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
			LoginResponse loginResponse =
				await _mediator.Send(new LoginRequest(claimsPrincipal), CancellationToken.None);
			return loginResponse.Code != Codes.Success.ToInt()
				? AuthenticateResult.Fail($"{loginResponse.Message} {loginResponse.Code}.")
				: AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
		}
		catch
		{
			return AuthenticateResult.Fail("Invalid Authorization Header");
		}
	}
}
