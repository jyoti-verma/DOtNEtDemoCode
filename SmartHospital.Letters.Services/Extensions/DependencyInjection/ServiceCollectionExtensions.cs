using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories.Extensions.DependencyInjection;
using SmartHospital.Letters.Services.CreateSnippets;

namespace SmartHospital.Letters.Services.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddLetterServices(this IServiceCollection services)
	{
		services.TryAddScoped<IEntityFactory, EntityFactory>();
		services.TryAddScoped<IDbInitializer, DbInitializer>();
		services.TryAddScoped<IJwtService, JwtService>();
		services.TryAddScoped<JwtSecurityTokenHandler>();
		services.TryAddScoped<ICreateSnippetsDispatcher, CreateSnippetsDispatcher>();
		services.TryAddScoped<ISnippedDtosService, SnippedDtosService>();
		services.RegisterSnippetStrategies(Assembly.GetExecutingAssembly());
		services.AddLetterRepositories();

		return services;
	}

	private static void RegisterSnippetStrategies(this IServiceCollection services, params Assembly[] assemblies)
	{
		Type strategyType = typeof(ICreateSnippetsStrategy);

		IEnumerable<Type> strategyImplementations = assemblies
			.SelectMany(assembly => assembly.GetTypes())
			.Where(type => strategyType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

		foreach (Type implementation in strategyImplementations)
		{
			services.TryAddScoped(strategyType, implementation);
			services.TryAddEnumerable(ServiceDescriptor.Scoped(strategyType, implementation));
		}
	}
}
