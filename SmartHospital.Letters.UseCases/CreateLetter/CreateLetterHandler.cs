using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Refit;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;
using SmartHospital.Letters.Fhir.Api.Client;
using SmartHospital.Letters.Fhir.Domain.Dtos;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.DefaultValues;

namespace SmartHospital.Letters.UseCases.CreateLetter;

public sealed class CreateLetterHandler : IRequestHandler<CreateLetterRequest, CreateLetterResponse>
{
	private readonly IEntityFactory _entityFactory;
	private readonly IFhirApiClient _fhirMockApiClient;
	private readonly ILetterRepository _letterRepository;
	private readonly ILetterTemplateRepository _letterTemplateRepository;
	private readonly SystemUser _user;

	public CreateLetterHandler(
		IEntityFactory entityFactory,
		IFhirApiClient fhirMockApiClient,
		ILetterTemplateRepository letterTemplateRepository,
		ILetterRepository letterRepository
	)
	{
		_entityFactory = entityFactory;
		_fhirMockApiClient = fhirMockApiClient;
		_letterTemplateRepository = letterTemplateRepository;
		_letterRepository = letterRepository;
		_user = new SystemUser();
	}

	public async Task<CreateLetterResponse> Handle(CreateLetterRequest request,
		CancellationToken cancellationToken = default)
	{
		PatientDto patientDto;
		try
		{
			patientDto = await _fhirMockApiClient
				.GetPatient(
					request.ExternalPatientId,
					cancellationToken
				);
		}
		catch (ValidationApiException exception) when (exception.StatusCode == HttpStatusCode.NotFound)
		{
			return new CreateLetterResponse((int)Codes.PatientNotFound, $"{request.ExternalPatientId} not found");
		}

		LetterTemplate? letterTemplate =
			await _letterTemplateRepository
				.GetByLetterTypeNameAsync(
					request.LetterTypeName,
					cancellationToken
				);

		if (letterTemplate is null)
		{
			return new CreateLetterResponse(
				(int)Codes.LetterTypeNotFound,
				$"{request.LetterTypeName} not found"
			);
		}

		Entities.Letter letter = _entityFactory.CreateLetter(
			Guid.NewGuid(),
			request.AdmissionType,
			letterTemplate.LetterType,
			patientDto.HumanNames.Single(p => p.Period?.End is null).GivenName,
			patientDto.HumanNames.Single(p => p.Period?.End is null).FamilyName,
			request.ExternalCaseNumber,
			patientDto.Identifier,
			LetterStatusTypes.InProgress,
			request.User,
			ToSections(letterTemplate.LetterTemplatesSectionTemplates)
		);

		try
		{
			int added = await _letterRepository.InsertAsync(letter, cancellationToken);
			if (added == 0)
			{
				throw new DbUpdateException("Nothing was inserted");
			}
		}
		catch (Exception ex)
		{
			return new CreateLetterResponse((int)Codes.LetterCreateFailed, ex.Message);
		}

		return new CreateLetterResponse(Dtos.Letter.FromLetterEntity(letter));
	}

	private List<Section> ToSections(
		IEnumerable<LetterTemplateSectionTemplate> letterTemplatesSectionTemplates)
	{
		return letterTemplatesSectionTemplates
			.Select(letterTemplateSectionTemplate => _entityFactory.CreateSection(
					Guid.NewGuid(),
					letterTemplateSectionTemplate.SectionTemplate.SectionType.DefaultTitle,
					letterTemplateSectionTemplate.SectionTemplate.SectionType,
					new Entities.Letter(),
					ToSnippets(letterTemplateSectionTemplate.SectionTemplate.SnippetTemplates),
					letterTemplateSectionTemplate.SortOrder,
					_user
				)
			)
			.OrderBy(p => p.SortOrder)
			.ToList();
	}

	private ICollection<Snippet> ToSnippets(IEnumerable<SnippetTemplate> snippetTemplates)
	{
		return snippetTemplates.Select(
				snippetTemplate =>
					_entityFactory.CreateSnippet(
						Guid.NewGuid(),
						snippetTemplate.Name,
						new Section(),
						snippetTemplate.SortOrder,
						_user,
						snippetTemplate.KeyValues.Select(
							keyValueTemplate => _entityFactory.CreateKeyValue(
								Guid.NewGuid(),
								new Snippet(),
								keyValueTemplate.SortOrder,
								keyValueTemplate.Value,
								keyValueTemplate.ValueType,
								keyValueTemplate.Key,
								keyValueTemplate.KeyType,
								_user
							)
						).ToList()
					)
			)
			.ToList();
	}
}
