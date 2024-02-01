using System.Globalization;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.UseCases.Extensions;

namespace SmartHospital.Letters.UseCases.Login;

public sealed class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
	private readonly UserManager<LetterUser> _userManager;

	public LoginHandler(UserManager<LetterUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
	{
		string userId = request
			.User
			.Claims
			.First(
				e => e.Type.Equals(
					ClaimTypes.NameIdentifier,
					StringComparison.InvariantCultureIgnoreCase
				)
			)
			.Value;
		LetterUser? user = await _userManager.FindByIdAsync(
			userId
		);

		if (user is null)
		{
			return new LoginResponse(
				Codes.UserNotFoundOnLogin.ToInt(),
				$"User {userId} not found"
			);
		}

		var token = new TokenResponse(
			request.User.Claims.First(e =>
				e.Type.Equals(nameof(Token.AccessToken), StringComparison.InvariantCultureIgnoreCase)).Value,
			request.User.Claims.First(e =>
				e.Type.Equals(nameof(Token.RefreshToken), StringComparison.InvariantCultureIgnoreCase)).Value
		);

		var exp = DateTime.Parse(
			request.User.Claims
				.First(
					e => e.Type.Equals(
						nameof(Token.RefreshTokenExpiration),
						StringComparison.InvariantCultureIgnoreCase
					)
				).Value,
			CultureInfo.InvariantCulture
		);

		user.RefreshToken = token.RefreshToken;
		user.RefreshTokenExpiryTime = exp;
		await _userManager.UpdateAsync(user);

		return new LoginResponse(
			token
		);
	}
}
