using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class SecondaryDiagnosisCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<SecondaryDiagnosisCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;

	public SecondaryDiagnosisCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<SecondaryDiagnosisCreateSnippetsStrategy> logger
	)
		: base(fhirApiClient)
	{
		_snippedDtosService = snippedDtosService;
		_logger = logger;
	}

	public override async Task<IEnumerable<Snippet>> CreateAsync(
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	)
	{
		var result = new List<Snippet>();

		try
		{
			IEnumerable<ConditionDto> conditions = await FhirApiClient
				.GetConditionsByObservationIdentifier(
					externalCaseNumber,
					cancellationToken
				);

			IEnumerable<ConditionDto> conditionDtos = conditions.ToList();
			if (!conditionDtos.Any())
			{
				return result;
			}

			IEnumerable<string> secondaryDiagnosis = conditionDtos
				.SelectMany(p => p.Codes
					.Where(q => q.Code == "secondary-diagnosis")
					.Select(s => s.Display)
				);
			var secondaryKeyValues = new List<KeyValue>
			{
				_snippedDtosService.CreateKeyValue("SecondaryDiagnosis",
					_snippedDtosService.CreateOrderedHtmlList(secondaryDiagnosis),
					1
				)
			};

			IEnumerable<string> drugIntolerances = conditionDtos
				.SelectMany(p => p.Codes
					.Where(q => q.Code == "drug-intolerance")
					.Select(s => s.Display)
				);

			secondaryKeyValues.Add(
				_snippedDtosService.CreateKeyValue("DrugIntolerance",
					_snippedDtosService.CreateUnorderedHtmlList(drugIntolerances),
					secondaryKeyValues.Count + 1
				)
			);

			result = new List<Snippet>
			{
				new()
				{
					Title = "Nebendiagnose(n)",
					KeyValues = new List<KeyValue>(secondaryKeyValues),
					SortOrder = 1
				}
			};
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating secondary diagnosis snippets");
		}

		return result;
	}
}
