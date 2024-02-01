using System.Reflection;
using System.Text.Json.Serialization;
using MediatR;
using SmartHospital.Letters.Api;
using SmartHospital.Letters.Api.Extensions.DependencyInjection;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Services;
using SmartHospital.Letters.UseCases.Extensions.DependencyInjection;

var assembly = Assembly.GetExecutingAssembly();
string version = $"v{assembly.GetName().Version?.ToString(3) ?? "1.0.0"}";
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddCustomAuthentication();
builder.Services.Configure<JwtServiceOptions>(
	builder.Configuration.GetSection(JwtServiceOptions.Section)
);
builder.Services.Configure<FhirApiOptions>(
	builder.Configuration.GetSection(FhirApiOptions.Section)
);

builder.Services.AddUseCases(builder.Configuration);
builder.Services.AddLetterConverter();

builder.Services
	.AddHealthChecks()
	.AddCheck<DbHealthCheck>(nameof(DbHealthCheck));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PipeLineStatusCodeBehavior<,>));

builder.Services.AddControllers().AddJsonOptions(options =>
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

builder.Services.AddLogging(config =>
{
	config.AddConfiguration(builder.Configuration.GetSection("Logging"));
	config.AddDebug();
	config.AddConsole();
});

#region CORS

/**************************************************/
// Has to be updated depending on the environment.
builder.Services.AddCors(options =>
{
	options.AddPolicy("Default",
		policy =>
		{
			policy.WithOrigins(
				"https://portal.azure.com",
				"https://ema.hosting.portal.azure.net"
			);
			policy.WithMethods("GET");
			policy.WithHeaders("*");
		});
});
/**************************************************/

#endregion

#endregion

#region Application

WebApplication app = builder.Build();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;
await services.GetRequiredService<IDbInitializer>().CheckAndUpdateDatabaseAsync();

// TODO: Remove this for production.
app.UseDeveloperExceptionPage();
app.MapHealthChecks("/health");
app.UseSwagger();
app.UseSwaggerUI(e => e.SwaggerEndpoint(
	"/swagger/" + version + "/swagger.json",
	assembly.GetName().Name + " " + version)
);
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

#endregion
