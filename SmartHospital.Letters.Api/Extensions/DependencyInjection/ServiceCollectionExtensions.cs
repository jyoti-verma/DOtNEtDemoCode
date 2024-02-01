using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SmartHospital.Letters.Api.Authentication;
using SmartHospital.Letters.Api.Services;
using SmartHospital.Letters.Api.Services.Swagger;
using SmartHospital.Letters.Services;

namespace SmartHospital.Letters.Api.Extensions.DependencyInjection;

internal static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
	{
		services.AddAuthentication()
			.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null)
			.AddScheme<AuthenticationSchemeOptions, BearerAuthenticationHandler>("Bearer", null);

		services.AddScoped<IOptions<JwtBearerOptions>>(sp =>
			{
				IOptions<JwtServiceOptions> jwtServiceOptions = sp.GetRequiredService<IOptions<JwtServiceOptions>>();

				return Options.Create(
					new JwtBearerOptions
					{
						SaveToken = true,
						RequireHttpsMetadata = false,
						TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidateAudience = false,
							ValidateIssuerSigningKey = true,
							ValidateLifetime = true,
							ValidAudience = jwtServiceOptions.Value.ValidAudience,
							ValidIssuer = jwtServiceOptions.Value.ValidIssuer,
							IssuerSigningKey = new SymmetricSecurityKey(
								Encoding.UTF8.GetBytes(jwtServiceOptions.Value.Key)
							),
							IssuerSigningKeyResolver = (_, _, _, parameters)
								=> new List<SecurityKey> { parameters.IssuerSigningKey },
							ClockSkew = TimeSpan.Zero
						}
					}
				);
			}
		);

		return services;
	}

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

			c.OperationFilter<AddParameterDescriptionOperationFilter>();

			string xmlFilename = $"{assembly.GetName().Name}.xml";
			c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

			c.UseInlineDefinitionsForEnums();

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

	public static IServiceCollection AddLetterConverter(this IServiceCollection services)
	{
		services.AddScoped<IConvertableLetter, ConvertableLetter>();
		services.AddScoped<IRazorService, RazorService>();
		return services;
	}
}
