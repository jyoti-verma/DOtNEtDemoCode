using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Services;

namespace SmartHospital.Letters.UseCases.RefreshToken;

public sealed class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IJwtService _jwtService;
	private readonly UserManager<LetterUser> _userManager;

	public RefreshTokenHandler(UserManager<LetterUser> userManager,
		IJwtService jwtService,
		IDateTimeProvider dateTimeProvider)
	{
		_userManager = userManager;
		_jwtService = jwtService;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		ClaimsPrincipal? principal = _jwtService.GetPrincipalFromExpiredToken(request.TokenResponse.AccessToken);
		if (principal is null)
		{
			return new RefreshTokenResponse((int)Codes.NotAuthorized, "Access denied");
		}

		string userId = principal
			.Claims
			.First(
				e => e.Type.Equals(
					ClaimTypes.NameIdentifier,
					StringComparison.InvariantCultureIgnoreCase
				)
			)
			.Value;

		LetterUser? letterUser = await _userManager.FindByIdAsync(userId);

		if (letterUser is null
		    || letterUser.RefreshToken != request.TokenResponse.RefreshToken
		    || letterUser.RefreshTokenExpiryTime <= _dateTimeProvider.Now)
		{
			return new RefreshTokenResponse((int)Codes.NotAuthorized, "Access denied");
		}

		Token newAccessToken = await _jwtService.GenerateTokenAsync(letterUser, cancellationToken);
		string newRefreshToken = _jwtService.GenerateRefreshToken();

		letterUser.RefreshToken = newRefreshToken;
		letterUser.RefreshTokenExpiryTime = newAccessToken.RefreshTokenExpiration;
		await _userManager.UpdateAsync(letterUser);

		return new RefreshTokenResponse(
			new TokenResponse(newAccessToken.AccessToken, newRefreshToken)
		);
	}
}
