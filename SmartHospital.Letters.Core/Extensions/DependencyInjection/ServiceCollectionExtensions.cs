using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SmartHospital.Letters.Core.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCoreServices(this IServiceCollection services)
	{
		services.TryAddScoped<IDateTimeProvider, DateTimeProvider>();

		return services;
	}
}
