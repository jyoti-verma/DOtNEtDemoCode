using System.Reflection;
using System.Text.Json.Serialization;
using SmartHospital.Fhir.Mock.Api.Extensions.DependencyInjection;
using SmartHospital.Letters.Fhir.Domain.Extensions.DependencyInjection;
using SmartHospital.Letters.Fhir.Domain.ExternalFhir;

var assembly = Assembly.GetExecutingAssembly();
string version = $"v{assembly.GetName().Version?.ToString(3) ?? "1.0.0"}";
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers().AddJsonOptions(options =>
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddFhir(builder.Configuration);
FhirConfiguration.Config = builder.Configuration;
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

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(e => e.SwaggerEndpoint(
	"/swagger/" + version + "/swagger.json", assembly.GetName().Name + " " + version));
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();
