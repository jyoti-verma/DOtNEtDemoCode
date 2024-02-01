using Microsoft.Extensions.Logging;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Domain;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.Extensions;

namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class DefaultSectionTypes : IDefaultValues
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<DefaultSectionTypes> _logger;
	private readonly IRepository<SectionType> _sectionTypeRepository;
	private readonly SystemUser _user;

	public DefaultSectionTypes(
		IRepository<SectionType> sectionTypeRepository,
		ILogger<DefaultSectionTypes> logger,
		IDateTimeProvider dateTimeProvider
	)
	{
		_sectionTypeRepository = sectionTypeRepository;
		_logger = logger;
		_dateTimeProvider = dateTimeProvider;
		_user = new SystemUser();
	}

	public async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		foreach (SectionType sectionType in SectionTypes(_dateTimeProvider.Now, _user))
		{
			if (await _sectionTypeRepository.TryAddAsync(sectionType, cancellationToken))
			{
				_logger.LogInformation("Added section type: {SectionTypeName}", sectionType.Name);
			}
		}
	}

	private static ICollection<SectionType> SectionTypes(DateTime created, LetterUser letterUser)
	{
		return new List<SectionType>
		{
			new(
				nameof(SectionTypeNames.Sender),
				SectionNames.Sender,
				new Guid("13767e47-48c0-4587-be6f-c2a027e60dd1"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.Address),
				SectionNames.Address,
				new Guid("56707aa6-4d00-4b59-ad2f-690af60619b1"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.Regarding),
				SectionNames.Regarding,
				new Guid("772f37cb-a1ce-49a9-b013-8e3605c30b08"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.Greeting),
				SectionNames.Greeting,
				new Guid("5b7f5a7e-7be6-40ce-8b0f-e4ede8f2fc28"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.OncologicalDiagnosis),
				SectionNames.OncologicalDiagnosis,
				new Guid("c50f018b-f691-40b8-b605-6a12bc296297"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.SecondaryDiagnosis),
				SectionNames.SecondaryDiagnosis,
				new Guid("96d6e5f9-ba66-44c0-b7e8-0d08efd730a9"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.PreviousHistoryOncologicalTherapy),
				SectionNames.PreviousHistoryOncologicalTherapy,
				new Guid("fd8ed300-feaa-4273-85fe-4be5dca3e2bc"),
				created,
				letterUser.UserName!
			),
			new(
				nameof(SectionTypeNames.Epicrisis),
				SectionNames.Epicrisis,
				new Guid("038588eb-38c8-466f-8acf-71f816ef3ced"),
				created,
				letterUser.UserName!
			)
		};
	}
}
