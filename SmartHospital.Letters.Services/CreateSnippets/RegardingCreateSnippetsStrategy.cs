using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class RegardingCreateSnippetsStrategy : AbstractCreateSnippetsStrategy
{
	private readonly ILogger<RegardingCreateSnippetsStrategy> _logger;
	private readonly ISnippedDtosService _snippedDtosService;


	public RegardingCreateSnippetsStrategy(
		IFhirApiClient fhirApiClient,
		ISnippedDtosService snippedDtosService,
		ILogger<RegardingCreateSnippetsStrategy> logger
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
			PatientDto patientDto = await FhirApiClient.GetPatient(externalPatientId, cancellationToken);

			var patientSnippet = new Snippet
			{
				Id = Guid.NewGuid(), Title = "Betreff", KeyValues = CreatePatientInfo(patientDto)
			};
			result.Add(patientSnippet);
		}
		catch (Exception exception)
		{
			_logger.LogError(exception, "Error while creating regarding snippets");
		}

		return result;
	}

	private ICollection<KeyValue> CreatePatientInfo(PatientDto patient)
	{
		HumanNameDto currentName = patient.HumanNames.Single(p => !p.Period!.End.HasValue);
		AddressDto currentAddress = patient.Addresses.Single(p => !p.Period!.End.HasValue);

		var result = new List<KeyValue>
		{
			_snippedDtosService.CreateKeyValue("Title", currentName.Prefix, 1),
			_snippedDtosService.CreateKeyValue("Firstname", currentName.GivenName, 2),
			_snippedDtosService.CreateKeyValue("Lastname", currentName.FamilyName, 3),
			_snippedDtosService.CreateKeyValue("Birthday", patient.DateOfBirth.ToShortDateString(), "DateTime", 4),
			_snippedDtosService.CreateKeyValue("Street", currentAddress.Lines.First(), 5),
			_snippedDtosService.CreateKeyValue("ZipCode", currentAddress.PostalCode, 5),
			_snippedDtosService.CreateKeyValue("City", currentAddress.City, 6),
			_snippedDtosService.CreateKeyValue("Country", currentAddress.Country, 7)
		};
		return result;
	}
}
