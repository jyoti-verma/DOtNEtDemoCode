using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;
using SmartHospital.Letters.Repositories;

namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class DefaultSectionTemplates : IDefaultValues
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IEntityFactory _entityFactory;
	private readonly ISectionTemplateRepository _sectionTemplateRepository;
	private readonly ISectionTypeRepository _sectionTypeRepository;
	private readonly SystemUser _user;

	public DefaultSectionTemplates(
		ISectionTypeRepository sectionTypeRepository,
		ISectionTemplateRepository sectionTemplateRepository,
		IEntityFactory entityFactory,
		IDateTimeProvider dateTimeProvider
	)
	{
		_sectionTypeRepository = sectionTypeRepository;
		_sectionTemplateRepository = sectionTemplateRepository;
		_entityFactory = entityFactory;
		_dateTimeProvider = dateTimeProvider;
		_user = new SystemUser();
	}

	public async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("2d997a0b-9556-40e9-b162-ee0e3f1cf286"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.Address,
					cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("786441ff-ae88-43f8-a71f-00b3d009db71"),
				sectionType:
				await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.Sender, cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("9e250a50-bd19-48a8-8c68-3b6b11bc740e"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.Regarding,
					cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("ec1c3084-643b-4e7f-b455-efdad7b41610"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.Greeting,
					cancellationToken),
				snippetTemplates: CreateGreetingSnippetTemplates(),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("40fff00c-1c65-4d16-9e10-5ee51b46f751"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.OncologicalDiagnosis,
					cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("644d83a0-4a6b-4f37-98df-74faf8aad5a0"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.SecondaryDiagnosis,
					cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("1f1d80af-380d-49dd-9b34-93c5e2043ca8"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(
					SectionNames.PreviousHistoryOncologicalTherapy, cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);

		await CreateSectionTemplate(
			new SectionTemplate(
				id: new Guid("c2d25f49-8806-457f-84d5-d95a9b912c4d"),
				sectionType: await _sectionTypeRepository.GetByDefaultTitleAsync(SectionNames.Epicrisis,
					cancellationToken),
				created: _dateTimeProvider.Now,
				createdBy: _user.UserName!
			)
		);
	}

	private async Task CreateSectionTemplate(SectionTemplate sectionTemplate)
	{
		SectionTemplate? template =
			await _sectionTemplateRepository.GetBySectionNameAsync(
				sectionTemplate.SectionType.DefaultTitle
			);

		if (template is null)
		{
			await _sectionTemplateRepository.InsertAsync(sectionTemplate);
		}
	}

	private ICollection<SnippetTemplate> CreateGreetingSnippetTemplates()
	{
		return new List<SnippetTemplate>
		{
			new(
				new Guid("d02ede04-f68f-4daa-8784-cdf9605109ca"),
				"GreetingText",
				new List<KeyValue>
				{
					_entityFactory.CreateKeyValue(
						Guid.NewGuid(),
						new Snippet(),
						1,
						"Sehr geehrte Damen und Herren,",
						"string",
						"Anrede",
						"string",
						new SystemUser()
					)
				},
				1
			)
		};
	}
}
