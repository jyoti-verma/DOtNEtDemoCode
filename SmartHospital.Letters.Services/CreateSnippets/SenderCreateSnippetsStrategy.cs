using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class SenderCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<SenderCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;

	public SenderCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<SenderCreateSnippetsStrategy> logger
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
		IEnumerable<Snippet> result = new List<Snippet>();
		try
		{
			ObservationDto observationDto =
				await FhirApiClient.GetObservation(
					externalCaseNumber,
					cancellationToken
				);

			PractitionerDto practitionerDto = await GetFirstPractitionerAsync(cancellationToken, observationDto);

			IEnumerable<OrganizationDto> organizationDtos =
				await FhirApiClient.GetOrganizations(
					practitionerDto.OrganizationIdentifier,
					cancellationToken
				);

			OrganizationDto? organizationDto = organizationDtos.SingleOrDefault();
			return organizationDto is null
				? result
				: CreateResultAsync(organizationDto, practitionerDto);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating sender snippets");
		}

		return result;
	}

	private List<Snippet> CreateResultAsync(OrganizationDto organizationDto, PractitionerDto practitionerDto)
	{
		return new List<Snippet>
		{
			new()
			{
				Title = "Absender",
				KeyValues = new List<KeyValue>
					{
						_snippedDtosService.CreateKeyValue("HospitalName", organizationDto.Name, 1),
						_snippedDtosService.CreateKeyValue(
							"DoctorAttending",
							_snippedDtosService.CreateHumanNameString(CurrentName(practitionerDto)),
							2
						)
					}.Concat(_snippedDtosService.CreateAddressKeyValues(organizationDto.Address))
					.ToList(),
				SortOrder = 1
			}
		};
	}

	private async Task<PractitionerDto> GetFirstPractitionerAsync(CancellationToken cancellationToken,
		ObservationDto observationDto)
	{
		IEnumerable<PractitionerDto> practitionerDtos =
			await FhirApiClient.GetPractitioners(
				observationDto.PerformerIdentifier,
				cancellationToken
			);

		return practitionerDtos.First();
	}

	private static HumanNameDto CurrentName(PractitionerDto practitionerDto)
	{
		return practitionerDto
			.HumanNames
			.Single(p => !p.Period!.End.HasValue);
	}
}
