using System.Globalization;
using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class OncologicalDiagnosisCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<OncologicalDiagnosisCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;

	public OncologicalDiagnosisCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<OncologicalDiagnosisCreateSnippetsStrategy> logger
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
		try
		{
			IEnumerable<DiagnosticReportDto> diagnosticReportDtos =
				await FhirApiClient.GetDiagnosticReportByPatientAndObservation(
					externalPatientId,
					externalCaseNumber,
					cancellationToken
				);

			return diagnosticReportDtos
				.OrderBy(p => p.EffectiveDateTime)
				.Select(diagnosticReportDto => new List<KeyValue>
				{
					_snippedDtosService.CreateKeyValue("HeaderDiagnosis", diagnosticReportDto.HeaderDiagnosis, 1),
					_snippedDtosService.CreateKeyValue("ICDCode", diagnosticReportDto.TumorEntity.Code, 2),
					_snippedDtosService.CreateKeyValue("ICDOCode", diagnosticReportDto.TumorMorphology.Code, 3),
					_snippedDtosService.CreateKeyValue("Histology", diagnosticReportDto.TumorHistology.Display, 4),
					_snippedDtosService.CreateKeyValue("MolecularPathology",
						_snippedDtosService.CreateUnorderedHtmlList(
							diagnosticReportDto.MolecularPathologyFindings.Select(p => p.Display)), 5),
					_snippedDtosService.CreateKeyValue("TumorStage",
						_snippedDtosService.CreateUnorderedHtmlList(
							diagnosticReportDto.TumorStadium.Select(p => p.Display)), 6),
					_snippedDtosService.CreateKeyValue("ECOGPerformanceStatus",
						diagnosticReportDto.EcogPerformanceStatus.ToString(CultureInfo.InvariantCulture), 7)
				})
				.Select(
					(keyValues, index) =>
						new Snippet
						{
							Title = "Onkologische Diagnose", KeyValues = keyValues.ToList(), SortOrder = index + 1
						}
				);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating snippets for oncological diagnosis");
		}

		return new List<Snippet>();
	}
}
