using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.Context.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddContext(
		this IServiceCollection services,
		IConfigurationRoot configuration
	)
	{
		services.AddDbContext<LetterDbContext>(options =>
			options.UseSqlite(configuration.GetConnectionString(nameof(LetterDbContext)))
		);
		services.AddIdentity<LetterUser, IdentityRole>(options =>
			{
				UpdatePasswordSettings(options);

				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
			})
			.AddEntityFrameworkStores<LetterDbContext>();

		services.AddScoped<UserManager<LetterUser>>();
		services.AddScoped<SignInManager<LetterUser>>();
		services.AddScoped<RoleManager<IdentityRole>>();

		services.AddScoped<CheckDbConnection>();


		return services;
	}

	private static void UpdatePasswordSettings(IdentityOptions options)
	{
		options.Password.RequireDigit = false;
		options.Password.RequireLowercase = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequiredLength = 6;
		options.Password.RequiredUniqueChars = 1;
	}
}
