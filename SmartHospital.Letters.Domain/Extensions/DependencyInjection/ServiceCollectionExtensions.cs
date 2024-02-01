using Microsoft.Extensions.DependencyInjection;

namespace SmartHospital.Letters.Domain.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLetters(this IServiceCollection services)
	{
		return services;
	}
}
