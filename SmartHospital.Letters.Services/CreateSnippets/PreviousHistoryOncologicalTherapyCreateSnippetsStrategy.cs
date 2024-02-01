using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class PreviousHistoryOncologicalTherapyCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<PreviousHistoryOncologicalTherapyCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;

	public PreviousHistoryOncologicalTherapyCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<PreviousHistoryOncologicalTherapyCreateSnippetsStrategy> logger
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
			IEnumerable<ConditionDto> conditionDtos = await FhirApiClient
				.GetConditionsByObservationIdentifier(
					externalCaseNumber,
					cancellationToken
				);

			IList<KeyValue> keyValues = new List<KeyValue>();
			foreach (ConditionDto conditionDto in conditionDtos)
			{
				keyValues.Add(
					_snippedDtosService.CreateKeyValue(
						"TimeInfo",
						_snippedDtosService.CreatePeriodString(conditionDto.RecordedDate),
						"DateTime",
						keyValues.Count + 1
					)
				);

				IEnumerable<CodingDto> codings =
					conditionDto.Codes.Where(p => p.Code == "secondary-diagnosis").ToList();
				string infoText = "";
				for (int i = 0; i < codings.Count(); i++)
				{
					infoText += codings.ElementAt(i).Display;

					if (i < codings.Count() - 1)
					{
						infoText += "<br/>";
					}
				}

				keyValues.Add(
					_snippedDtosService.CreateKeyValue("InfoText", infoText, keyValues.Count + 1)
				);

				result.Add(new Snippet
				{
					Title = "Bisheriger Verlauf", KeyValues = keyValues, SortOrder = result.Count + 1
				});
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating snippets for previous history oncological therapy");
		}

		return result;
	}
}
