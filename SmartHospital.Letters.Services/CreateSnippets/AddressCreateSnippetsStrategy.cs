using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class AddressCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ISnippedDtosService _snippedDtosService;

	public AddressCreateSnippetsStrategy(IFhirApiClient fhirApiClient, ISnippedDtosService snippedDtosService)
		: base(fhirApiClient)
	{
		_snippedDtosService = snippedDtosService;
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
			PatientDto patient = await FhirApiClient.GetPatient(externalPatientId, cancellationToken);
			HumanNameDto currentName = patient.HumanNames.Single(p => !p.Period!.End.HasValue);
			AddressDto currentAddress = patient.Addresses.Single(p => !p.Period!.End.HasValue);

			result.Add(
				CreatePatientSnippet(
					patient,
					currentName,
					currentAddress
				)
			);

			IEnumerable<ObservationDto> observations =
				await FhirApiClient.GetObservations(externalPatientId, cancellationToken);
			foreach (ObservationDto observationDto in observations)
			{
				IEnumerable<PractitionerDto> practitionerDtos =
					await FhirApiClient.GetPractitioners(
						observationDto.PerformerIdentifier,
						cancellationToken
					);
				PractitionerDto? practitionerDto = practitionerDtos.SingleOrDefault();
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

				var organizationSnippet = new Snippet
				{
					Title = "Anschrift",
					KeyValues = new List<KeyValue>
					{
						_snippedDtosService.CreateKeyValue("AddressType", organizationDto.Type.Display, 1),
						_snippedDtosService.CreateKeyValue("OfficeDoctor", organizationDto.Name, 2)
					}
				};

				organizationSnippet.KeyValues = organizationSnippet.KeyValues
					.Concat(
						_snippedDtosService.CreateAddressKeyValues(organizationDto.Address)
					)
					.ToList();

				result.Add(organizationSnippet);
			}
		}
		catch (Exception)
		{
			// ignored an return empty result
		}

		return result;
	}

	private Snippet CreatePatientSnippet(PatientDto patient, HumanNameDto humanNameDto, AddressDto addressDto)
	{
		var keyValues = new List<KeyValue>
		{
			_snippedDtosService.CreateKeyValue("Salutation", _snippedDtosService.GetSalutation(patient.Gender), 1),
			_snippedDtosService.CreateKeyValue("Title", humanNameDto.Prefix, 2),
			_snippedDtosService.CreateKeyValue("Firstname", humanNameDto.GivenName, 3),
			_snippedDtosService.CreateKeyValue("Lastname", humanNameDto.FamilyName, 4)
		};
		keyValues.AddRange(_snippedDtosService.CreateAddressKeyValues(addressDto));

		return new Snippet { Title = "Anschrift", KeyValues = keyValues, SortOrder = 1 };
	}
}
