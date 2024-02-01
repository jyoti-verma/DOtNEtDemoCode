using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.DataExtraction;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir.Repo;

namespace SmartHospital.Letters.Fhir.Domain.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddFhir(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<IGlobalRepo, ExternalFhirRepo>();
		services.AddScoped<IExternalFhirRepo, ExternalFhirRepo>();
		services.AddScoped<IFhirRepository, FhirRepository>();
		services.AddScoped<FhirRepoManager>();
		services.AddScoped<ExternalFhirClient>();
		services.AddScoped<FhirPatientDataExtraction>();
		services.AddScoped<FhirObservationDataExtraction>();
		services.AddScoped<FhirOrganizationDataExtraction>();
		services.AddScoped<FhirConditionDataExtraction>();
		services.AddScoped<FhirPractitionerDataExtraction>();
		services.AddScoped<FhirDiagnosticReportDataExtraction>();

		return services;
	}
}
