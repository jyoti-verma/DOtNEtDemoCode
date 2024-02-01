using System.Reflection;
using Microsoft.OpenApi.Models;

namespace SmartHospital.Fhir.Mock.Api.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
	{
		var assembly = Assembly.GetExecutingAssembly();
		string version = $"v{assembly.GetName().Version?.ToString(3) ?? "1.0.0"}";

		services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc(
				version,
				new OpenApiInfo { Title = assembly.GetName().Name, Version = version }
			);
			// using System.Reflection;
			string xmlFilename = $"{assembly.GetName().Name}.xml";
			c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

			c.AddSecurityDefinition(
				"Bearer",
				new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme."
				}
			);
			c.AddSecurityDefinition(
				"Basic",
				new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "basic",
					In = ParameterLocation.Header,
					Description = "Basic Authorization header using the Bearer scheme."
				}
			);
			c.AddSecurityRequirement(
				new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
						},
						Array.Empty<string>()
					}
				}
			);
			c.AddSecurityRequirement(
				new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
						},
						Array.Empty<string>()
					}
				}
			);
		});
		return services;
	}
}
