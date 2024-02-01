using Faker;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class EpicrisisCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly string[] _sections =
	{
		"Grund der Einweisung", "Verlauf", "Erwähnenswerte Laborbefunde", "Therapie", "Untersuchungen/Befunde",
		"Anamnese/ECOG Status"
	};

	private readonly ISnippedDtosService _snippedDtosService;

	public EpicrisisCreateSnippetsStrategy(IFhirApiClient fhirApiClient, ISnippedDtosService snippedDtosService)
		: base(fhirApiClient)
	{
		_snippedDtosService = snippedDtosService;
	}

	public override Task<IEnumerable<Snippet>> CreateAsync(
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	)
	{
		var rnd = new Random();

		var result = _sections
			.Select(section => new Snippet
			{
				Title = section,
				KeyValues = new List<KeyValue>
				{
					_snippedDtosService.CreateKeyValue("InfoText", Lorem.Paragraph(rnd.Next(1, 5)), 1)
				},
				SortOrder = 1
			})
			.ToList();

		return Task.FromResult<IEnumerable<Snippet>>(result);
	}
}
