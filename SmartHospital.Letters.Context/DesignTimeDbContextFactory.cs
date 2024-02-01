using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SmartHospital.Letters.Context;

internal sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LetterDbContext>
{
	public LetterDbContext CreateDbContext(string[] args)
	{
		IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json")
			.AddJsonFile(
				$"appsettings{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
				true
			)
			.Build();
		var builder = new DbContextOptionsBuilder<LetterDbContext>();

		builder.UseSqlite(configuration.GetConnectionString(nameof(LetterDbContext)));
		builder.EnableSensitiveDataLogging();

		return new LetterDbContext(builder.Options);
	}
}
