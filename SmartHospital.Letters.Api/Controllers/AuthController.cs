using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.UseCases.Login;
using SmartHospital.Letters.UseCases.Logout;
using SmartHospital.Letters.UseCases.RefreshToken;

namespace SmartHospital.Letters.Api.Controllers;

/// <summary>
///     Controller for authentication
/// </summary>
[Route("[controller]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
	private readonly IMediator _mediator;

	/// <summary>
	///     Initializes a new instance of the <see cref="AuthController" /> class.
	/// </summary>
	/// <param name="mediator"></param>
	public AuthController(
		IMediator mediator
	)
	{
		_mediator = mediator;
	}

	/// <summary>
	///     Login by username and password
	/// </summary>
	/// <param name="cancellationToken">CancellationToken</param>
	/// <returns>a JWT token</returns>
	/// <remarks>
	///     Sample request:
	///     Login as Doctor
	///     POST /login
	///     "userName": "ghouse",
	///     "password": "secret!123"
	///     Login as AdministrationOffice User
	///     POST /login
	///     "userName": "jdoe",
	///     "password": "admin!456"
	/// </remarks>
	[Authorize(AuthenticationSchemes = "Basic")]
	[HttpPost]
	[Route("Login")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<LoginResponse> Login(CancellationToken cancellationToken)
	{
		return await _mediator.Send(new LoginRequest(User), cancellationToken);
	}

	/// <summary>
	///     Logout
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[Authorize(AuthenticationSchemes = "Bearer")]
	[HttpGet]
	[Route("Logout")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<LogoutResponse> Logout(CancellationToken cancellationToken)
	{
		return await _mediator.Send(new LogoutRequest(User), cancellationToken);
	}

	/// <summary>
	///     Refresh
	/// </summary>
	/// <param name="tokenResponse"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	[HttpPost]
	[Route("RefreshToken")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<RefreshTokenResponse> RefreshToken(TokenResponse tokenResponse,
		CancellationToken cancellationToken)
	{
		return await _mediator.Send(new RefreshTokenRequest(tokenResponse), cancellationToken);
	}
}
