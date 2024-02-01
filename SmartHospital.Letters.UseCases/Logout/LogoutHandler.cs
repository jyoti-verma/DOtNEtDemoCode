using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.UseCases.Extensions;

namespace SmartHospital.Letters.UseCases.Logout;

public sealed class LogoutHandler : IRequestHandler<LogoutRequest, LogoutResponse>
{
	private readonly SignInManager<LetterUser> _signInManager;
	private readonly UserManager<LetterUser> _userManager;

	public LogoutHandler(UserManager<LetterUser> userManager, SignInManager<LetterUser> signInManager)
	{
		_userManager = userManager;
		_signInManager = signInManager;
	}

	public async Task<LogoutResponse> Handle(LogoutRequest request, CancellationToken cancellationToken)
	{
		await _signInManager.SignOutAsync();

		string userId = request.User.Claims.First(e =>
			e.Type.Equals(ClaimTypes.NameIdentifier, StringComparison.InvariantCultureIgnoreCase)).Value;
		LetterUser? user = await _userManager.FindByIdAsync(
			userId
		);

		if (user is null)
		{
			return new LogoutResponse(
				Codes.UserNotFoundOnLogin.ToInt(),
				$"User {userId} not found"
			);
		}

		user.RefreshToken = null;
		user.RefreshTokenExpiryTime = null;

		await _userManager.UpdateAsync(user);

		return new LogoutResponse();
	}
}
