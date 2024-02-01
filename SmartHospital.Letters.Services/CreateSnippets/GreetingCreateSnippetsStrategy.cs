using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

/// <summary>
///     Create greeting snippets.
/// </summary>
public sealed class GreetingCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<GreetingCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;

	/// <summary>
	///     Create greeting snippets.
	/// </summary>
	/// <param name="fhirApiClient"></param>
	/// <param name="snippedDtosService"></param>
	/// <param name="logger"></param>
	public GreetingCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<GreetingCreateSnippetsStrategy> logger
	)
		: base(fhirApiClient)
	{
		_snippedDtosService = snippedDtosService;
		_logger = logger;
	}

	/// <summary>
	///     Should return a list of snippets, based on the given parameters and the strategy.
	/// </summary>
	/// <param name="externalPatientId"></param>
	/// <param name="externalCaseNumber"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public override async Task<IEnumerable<Snippet>> CreateAsync(
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	)
	{
		var snippets = new List<Snippet>();

		try
		{
			PatientDto patient = await FhirApiClient.GetPatient(externalPatientId, cancellationToken);
			HumanNameDto currentName = patient.HumanNames.Single(p => !p.Period!.End.HasValue);
			string patientName = $"{currentName.Prefix} {currentName.GivenName} {currentName.FamilyName}";

			IEnumerable<ObservationDto> observations =
				await FhirApiClient.GetObservations(externalPatientId, cancellationToken);
			IEnumerable<ObservationDto> observationDtos = observations.ToList();
			ObservationDto lastObservation = observationDtos.OrderByDescending(p => p.EffectiveDateTime).First();

			string txt = $"{_snippedDtosService.GetSalutationPerson(patient.Gender)} {currentName.FamilyName},<br/>" +
			             $"wir berichten nachfolgend über die am {lastObservation.EffectiveDateTime:dd.MM.yyyy} " +
			             $"erfolgte {_snippedDtosService.GetCategory(lastObservation.Category)} Behandlung:";

			snippets.Add(
				new Snippet
				{
					Title = "Begrüßung Patient",
					KeyValues = new List<KeyValue>
					{
						_snippedDtosService.CreateKeyValue("GreetingText", txt, snippets.Count + 1)
					},
					SortOrder = snippets.Count + 1
				});


			foreach (ObservationDto observationDto in observationDtos)
			{
				IEnumerable<PractitionerDto> practitionersDto =
					await FhirApiClient.GetPractitioners(
						observationDto.PerformerIdentifier,
						cancellationToken
					);
				PractitionerDto? practitionerDto = practitionersDto.SingleOrDefault();
				if (practitionerDto is null)
				{
					continue;
				}

				IEnumerable<OrganizationDto> organizationDtos =
					await FhirApiClient.GetOrganizations(
						practitionerDto.OrganizationIdentifier,
						cancellationToken
					);

				OrganizationDto? organizationDto = organizationDtos.SingleOrDefault();
				if (organizationDto is null)
				{
					continue;
				}

				string doctorTxt =
					$"Sehr geehrte Damen und Herren Kollegen,<br/>wir berichten über den gemeinsamen Patient" +
					$" {patientName.Trim()} welcher sich am {lastObservation.EffectiveDateTime:dd.MM.yyyy} " +
					$"in {_snippedDtosService.GetCategory(lastObservation.Category)} Behandlung befand:";
				snippets.Add(
					new Snippet
					{
						Title = $"Begrüßung {organizationDto.Name}",
						KeyValues = new List<KeyValue>
						{
							_snippedDtosService.CreateKeyValue("GreetingText", doctorTxt, 1)
						},
						SortOrder = snippets.Count + 1
					});
			}
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating greeting snippets");
		}

		return snippets;
	}
}
