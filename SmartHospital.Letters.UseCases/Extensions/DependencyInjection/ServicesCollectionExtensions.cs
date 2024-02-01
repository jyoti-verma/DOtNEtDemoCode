using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHospital.Letters.Context.Extensions.DependencyInjection;
using SmartHospital.Letters.Fhir.Api.Client.Extensions.DependencyInjection;
using SmartHospital.Letters.Services.Extensions.DependencyInjection;

namespace SmartHospital.Letters.UseCases.Extensions.DependencyInjection;

public static class ServicesCollectionExtensions
{
	public static IServiceCollection AddUseCases(this IServiceCollection services,
		IConfigurationRoot configuration)
	{
		services.AddMediatR(
			cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly)
		);
		services.AddLetterServices();
		services.AddContext(configuration);
		services.AddFhirMockApiServices();

		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipeLineLoggingBehavior<,>));

		return services;
	}
}
