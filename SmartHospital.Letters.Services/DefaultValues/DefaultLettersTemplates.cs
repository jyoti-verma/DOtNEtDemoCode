using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.Extensions;

namespace SmartHospital.Letters.Services.DefaultValues;

internal sealed class DefaultLettersTemplates : IDefaultValues
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILetterTemplateRepository _letterTemplateRepository;
	private readonly ILetterTemplateSectionTemplateRepository _letterTemplateSectionTemplateRepository;
	private readonly ILetterTypeRepository _letterTypeRepository;
	private readonly ISectionTemplateRepository _sectionTemplateRepository;
	private readonly SystemUser _user;

	public DefaultLettersTemplates(
		ILetterTypeRepository letterTypeRepository,
		ISectionTemplateRepository sectionTemplateRepository,
		ILetterTemplateSectionTemplateRepository letterTemplateSectionTemplateRepository,
		ILetterTemplateRepository letterTemplateRepository,
		IDateTimeProvider dateTimeProvider
	)
	{
		_letterTypeRepository = letterTypeRepository;
		_sectionTemplateRepository = sectionTemplateRepository;
		_letterTemplateSectionTemplateRepository = letterTemplateSectionTemplateRepository;
		_letterTemplateRepository = letterTemplateRepository;
		_dateTimeProvider = dateTimeProvider;
		_user = new SystemUser();
	}

	public async Task CreateAsync(CancellationToken cancellationToken = default)
	{
		await AssignSectionsForTherapyAsync(
			await GetOrCreateLetterTemplateByLetterTypeNameAsync(
				new LetterTemplate(
					_dateTimeProvider.Now,
					_user.UserName!,
					await _letterTypeRepository.GetByNameOrThrowAsync(
						LetterTypes.Therapy,
						cancellationToken
					)
				)
			)
		);

		await AssignSectionsForImagingFollowUpControl(
			await GetOrCreateLetterTemplateByLetterTypeNameAsync(
				new LetterTemplate(
					_dateTimeProvider.Now,
					_user.UserName!,
					await _letterTypeRepository.GetByNameOrThrowAsync(
						LetterTypes.ImagingFollowUpControl,
						cancellationToken
					)
				)
			)
		);

		await AssignSectionsForAftercare(
			await GetOrCreateLetterTemplateByLetterTypeNameAsync(
				new LetterTemplate(
					_dateTimeProvider.Now,
					_user.UserName!,
					await _letterTypeRepository.GetByNameOrThrowAsync(
						LetterTypes.Aftercare,
						cancellationToken
					)
				)
			)
		);

		await AssignSectionsForTerminationOfTumorRelatedTherapyAsync(
			await GetOrCreateLetterTemplateByLetterTypeNameAsync(
				new LetterTemplate(
					_dateTimeProvider.Now,
					_user.UserName!,
					await _letterTypeRepository.GetByNameOrThrowAsync(
						LetterTypes.TerminationOfTumorRelatedTherapy,
						cancellationToken
					)
				)
			)
		);
	}

	private async Task AssignSectionsForTherapyAsync(LetterTemplate letterTemplate)
	{
		await InsertAssignments(
			letterTemplate,
			SectionNames.Sender,
			SectionNames.Address,
			SectionNames.Regarding,
			SectionNames.Greeting,
			SectionNames.SecondaryDiagnosis,
			SectionNames.Epicrisis
		);
	}

	private async Task InsertAssignments(LetterTemplate letterTemplate, params string[] sectionNames)
	{
		int order = 1;
		foreach (string name in sectionNames)
		{
			await InsertAssignment(
				letterTemplate,
				await _sectionTemplateRepository.GetBySectionNameOrThrowAsync(name),
				order++
			);
		}
	}

	private async Task AssignSectionsForImagingFollowUpControl(LetterTemplate letterTemplate)
	{
		await InsertAssignments(
			letterTemplate,
			SectionNames.Sender,
			SectionNames.Address,
			SectionNames.Regarding,
			SectionNames.Greeting,
			SectionNames.OncologicalDiagnosis,
			SectionNames.SecondaryDiagnosis,
			SectionNames.PreviousHistoryOncologicalTherapy
		);
	}

	private async Task AssignSectionsForAftercare(LetterTemplate letterTemplate)
	{
		await InsertAssignments(
			letterTemplate,
			SectionNames.Sender,
			SectionNames.Address,
			SectionNames.Regarding,
			SectionNames.Greeting,
			SectionNames.OncologicalDiagnosis,
			SectionNames.SecondaryDiagnosis,
			SectionNames.PreviousHistoryOncologicalTherapy,
			SectionNames.PreviousHistoryOncologicalTherapy
		);
	}

	private async Task AssignSectionsForTerminationOfTumorRelatedTherapyAsync(LetterTemplate letterTemplate)
	{
		await InsertAssignments(
			letterTemplate,
			SectionNames.Sender,
			SectionNames.Address,
			SectionNames.Regarding,
			SectionNames.Greeting,
			SectionNames.OncologicalDiagnosis,
			SectionNames.SecondaryDiagnosis,
			SectionNames.Epicrisis
		);
	}

	private async Task InsertAssignment(
		LetterTemplate letterTemplate,
		SectionTemplate sectionTemplate,
		int sortOrder,
		CancellationToken cancellationToken = default
	)
	{
		LetterTemplateSectionTemplate? entity = await _letterTemplateSectionTemplateRepository.GetByAsync(
			letterTemplate.Id,
			sectionTemplate.Id,
			cancellationToken
		);

		if (entity is null)
		{
			entity = new LetterTemplateSectionTemplate(
				sortOrder,
				_dateTimeProvider.Now,
				_user.UserName!,
				letterTemplate,
				sectionTemplate
			);

			await _letterTemplateSectionTemplateRepository.TryAddAsync(entity, cancellationToken);
		}
	}

	private async Task<LetterTemplate> GetOrCreateLetterTemplateByLetterTypeNameAsync(LetterTemplate letterTemplate)
	{
		LetterTemplate? template = await _letterTemplateRepository.GetByLetterTypeNameAsync(
			letterTemplate.LetterType.Name
		);

		if (template is not null)
		{
			return template;
		}

		await _letterTemplateRepository.InsertAsync(letterTemplate);

		return letterTemplate;
	}
}
