using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class DefaultUsers : IDefaultValues
{
	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly UserManager<LetterUser> _userManager;

	public DefaultUsers(
		RoleManager<IdentityRole> roleManager,
		UserManager<LetterUser> userManager
	)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		LetterUser user = await GetOrCreateLetterUserAsync();
		await CreateRolesForUser(user);

		LetterUser doctor = await GetOrCreateDoctorAsync();
		await CreateClaimsAndRolesForDoctor(doctor);
	}

	/// <summary>
	///     TODO: Remove for Production
	/// </summary>
	/// <returns></returns>
	private async Task<LetterUser> GetOrCreateDoctorAsync()
	{
		LetterUser? doc = await _userManager.FindByNameAsync("ghouse");
		if (doc is not null)
		{
			return doc;
		}

		LetterUser newDoc = new()
		{
			UserName = "ghouse",
			Firstname = "Gregory",
			Lastname = "House",
			Title = "Dr.",
			Email = "drhouse@hospital.com",
			EmailConfirmed = true,
			Salutation = "Herr"
		};
		await _userManager.CreateAsync(newDoc, "secret!123");

		string userName = "ghouse";
		return await _userManager.FindByNameAsync(userName) ?? throw new UserNotFoundException(userName);
	}

	private async Task CreateRolesForUser(LetterUser user)
	{
		if (!await _roleManager.RoleExistsAsync(UserRoles.AdministrationOffice))
		{
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.AdministrationOffice));
		}

		if (!await _userManager.IsInRoleAsync(user, UserRoles.AdministrationOffice))
		{
			await _userManager.AddToRoleAsync(user, UserRoles.AdministrationOffice);
		}

		await AddMissingClaims(UserRoles.AdministrationOffice, AdministrationOfficeClaims());
	}

	private async Task<LetterUser> GetOrCreateLetterUserAsync()
	{
		LetterUser? user = await _userManager.FindByNameAsync("jdoe");
		if (user is not null)
		{
			return user;
		}

		LetterUser newUser = new()
		{
			UserName = "jdoe",
			Firstname = "John",
			Lastname = "Doe",
			Email = "jdoe@hospital.com",
			EmailConfirmed = true,
			Salutation = "Herr"
		};

		await _userManager.CreateAsync(newUser, "admin!456");
		return await _userManager.FindByNameAsync("jdoe") ?? throw new UserNotFoundException("User not found");
	}

	private async Task CreateClaimsAndRolesForDoctor(LetterUser defaultDoctor)
	{
		await CreateDoctorRole();
		await AssignDoctorRole(defaultDoctor);
		await AddMissingClaims(UserRoles.Doctor, DoctorClaims());
	}

	private async Task CreateDoctorRole()
	{
		if (!await _roleManager.RoleExistsAsync(UserRoles.Doctor))
		{
			await _roleManager.CreateAsync(new IdentityRole(UserRoles.Doctor));
		}
	}

	private async Task AssignDoctorRole(LetterUser defaultDoctor)
	{
		if (!await _userManager.IsInRoleAsync(defaultDoctor, UserRoles.Doctor))
		{
			await _userManager.AddToRoleAsync(defaultDoctor, UserRoles.Doctor);
		}
	}

	private async Task AddMissingClaims(string roleName, IList<Claim> claims)
	{
		IdentityRole role = _roleManager.Roles.Single(p => p.Name == roleName);

		IList<Claim> roleClaims = await _roleManager.GetClaimsAsync(role);

		IEnumerable<Claim> newClaims = claims.Where(p =>
			!roleClaims.Any(q => q.Type == p.Type
			                     && q.Value == p.Value));
		foreach (Claim claim in newClaims)
		{
			await _roleManager.AddClaimAsync(role, claim);
		}
	}

	private static List<Claim> DoctorClaims()
	{
		return new List<Claim>
		{
			new("permission", "letter.read"),
			new("permission", "letter.create"),
			new("permission", "letter.edit"),
			new("permission", "letter.validate"),
			new("permission", "letter.print"),
			new("permission", "letter.generate_snippets")
		};
	}

	private static IList<Claim> AdministrationOfficeClaims()
	{
		return new List<Claim> { new("permission", "letter.print"), new("permission", "letter.read") };
	}
}
