using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace SmartHospital.Letters.Fhir.Api.Client.Extensions.DependencyInjection;

public static class ServicesCollectionExtensions
{
	public static IServiceCollection AddFhirMockApiServices(this IServiceCollection services)
	{
		services
			.AddRefitClient<IFhirApiClient>()
			.ConfigureHttpClient((sp, p) =>
			{
				FhirApiOptions opt = sp.GetRequiredService<IOptions<FhirApiOptions>>().Value;
				p.Timeout = TimeSpan.Parse(opt.Timeout, CultureInfo.InvariantCulture);
				p.BaseAddress = new Uri(opt.HostUrl);
			});

		return services;
	}
}
