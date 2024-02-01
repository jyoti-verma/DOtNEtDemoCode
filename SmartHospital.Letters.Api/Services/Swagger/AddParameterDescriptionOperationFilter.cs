using Microsoft.OpenApi.Models;
using SmartHospital.Letters.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartHospital.Letters.Api.Services.Swagger;

internal sealed class AddParameterDescriptionOperationFilter : IOperationFilter
{
	private readonly ILogger<AddParameterDescriptionOperationFilter> _logger;
	private readonly IServiceScopeFactory _serviceScopeFactory;


	public AddParameterDescriptionOperationFilter(
		ILogger<AddParameterDescriptionOperationFilter> logger,
		IServiceScopeFactory serviceScopeFactory
	)

	{
		_logger = logger;
		_serviceScopeFactory = serviceScopeFactory;
	}

	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		AddLetterTypeNames(operation);
	}

	private void AddLetterTypeNames(OpenApiOperation operation)
	{
		const string parameterName = "letterTypeName";
		using IServiceScope scope = _serviceScopeFactory.CreateScope();
		ILetterTemplateRepository repository = scope.ServiceProvider.GetRequiredService<ILetterTemplateRepository>();

		OpenApiParameter? param = operation.Parameters.SingleOrDefault(p =>
			p.Name.Equals(parameterName, StringComparison.OrdinalIgnoreCase));
		if (param is null)
		{
			return;
		}
		_logger.LogDebug("Create list of available letter type names for operation {OperationId}", operation.OperationId);

		IQueryable<string> values = repository.All()
			.Select(l => l.LetterType.Name)
			.OrderBy(e => e)
			.Distinct();

		param.Description += $"\n <i>Available values:</i> &quot;{string.Join("&quot;, &quot;", values)}&quot;";
	}
}
