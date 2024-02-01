using MediatR;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;

namespace SmartHospital.Letters.UseCases.UpdateLetter;

public sealed class UpdateLetterHandler : IRequestHandler<UpdateLetterRequest, UpdateLetterResponse>
{
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IEntityFactory _entityFactory;
	private readonly IRepository<KeyValue> _keyValueRepository;
	private readonly ILetterRepository _letterRepository;
	private readonly IRepository<Section> _sectionRepository;
	private readonly IRepository<SectionType> _sectionTypeRepository;
	private readonly IRepository<Snippet> _snippetRepository;
	private UpdateLetterRequest? _request;

	public UpdateLetterHandler(
		ILetterRepository letterRepository,
		IRepository<Section> sectionRepository,
		IRepository<Snippet> snippetRepository,
		IRepository<KeyValue> keyValueRepository,
		IRepository<SectionType> sectionTypeRepository,
		IDateTimeProvider dateTimeProvider,
		IEntityFactory entityFactory
	)
	{
		_letterRepository = letterRepository;
		_sectionRepository = sectionRepository;
		_snippetRepository = snippetRepository;
		_keyValueRepository = keyValueRepository;
		_sectionTypeRepository = sectionTypeRepository;
		_dateTimeProvider = dateTimeProvider;
		_entityFactory = entityFactory;
	}

	public async Task<UpdateLetterResponse> Handle(
		UpdateLetterRequest request,
		CancellationToken cancellationToken = default
	)
	{
		_request = request;
		Entities.Letter? letter = await _letterRepository
			.GetByIdAsync(request.Letter.Id, cancellationToken);
		if (letter is null)
		{
			return new UpdateLetterResponse(
				(int)Codes.LetterDoesNotExists,
				$"letter with {request.Letter.Id} not found");
		}

		await HandleSectionsAsync(letter, request.Letter, cancellationToken);

		letter.Status = (LetterStatusTypes)request.Letter.Status;
		letter.Modified = _dateTimeProvider.Now;
		letter.ModifiedBy = request.User.UserName!;

		await _letterRepository.SaveChangesAsync(cancellationToken);

		return new UpdateLetterResponse(Dtos.Letter.FromLetterEntity(
				await _letterRepository
					.GetByIdAsync(request.Letter.Id, cancellationToken)
				?? throw new InvalidOperationException("Could not find letter after update")
			)
		);
	}

	private async Task HandleSectionsAsync(
		Entities.Letter letterEntity,
		Dtos.Letter letterDto,
		CancellationToken cancellationToken
	)
	{
		RemoveDeleted(letterEntity.Sections, letterDto.Sections);
		await UpdateSectionsAsync(letterEntity.Sections, letterDto.Sections, cancellationToken);
		await AddSectionsAsync(letterEntity, letterDto.Sections, cancellationToken);
	}

	internal static void RemoveDeleted<T, TIncoming>(ICollection<T> existing, ICollection<TIncoming> incoming)
		where T : IIdentifier<Guid>
		where TIncoming : IIdentifier<Guid>
	{
		var nonExistingEntities = existing.GroupJoin(
				incoming,
				entity => entity.Id,
				dto => dto.Id,
				(entity, dtos) => new { Entity = entity, Dtos = dtos }
			)
			.Where(joinResult => !joinResult.Dtos.Any())
			.Select(joinResult => joinResult.Entity)
			.ToList();

		foreach (T item in nonExistingEntities)
		{
			existing.Remove(item);
		}
	}

	private async Task HandleSnippetsAsync(
		Section sectionEntity,
		Dtos.Section sectionDto,
		CancellationToken cancellationToken
	)
	{
		RemoveDeleted(sectionEntity.Snippets, sectionDto.Snippets);
		await UpdateSnippetsAsync(sectionEntity.Snippets, sectionDto.Snippets, cancellationToken);
		await AddSnippetsAsync(sectionEntity, sectionDto.Snippets, cancellationToken);
	}

	private async Task HandleKeyValuesAsync(
		Snippet snippetEntity,
		Dtos.Snippet snippetDto,
		CancellationToken cancellationToken
	)
	{
		RemoveDeleted(snippetEntity.KeyValues, snippetDto.KeyValues);
		await UpdateKeyValuesAsync(snippetEntity.KeyValues, snippetDto.KeyValues, cancellationToken);
		await AddKeyValuesAsync(snippetEntity, snippetDto.KeyValues, cancellationToken);
	}


	private static IEnumerable<TDto> NewItems<TEntity, TDto>(IEnumerable<TEntity> current, IEnumerable<TDto> other)
		where TEntity : IIdentifier<Guid>
		where TDto : IIdentifier<Guid>
	{
		return other.Where(dto => current.All(entity => entity.Id != dto.Id));
	}

	private async Task AddSectionsAsync(
		Entities.Letter letter,
		IEnumerable<Dtos.Section> dtos,
		CancellationToken cancellationToken
	)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		var sectionTypeLookup = _sectionTypeRepository.All()
			.ToDictionary(e => e.Id, e => e);

		await _sectionRepository.AddRangeAsync(
			NewItems(letter.Sections, dtos)
				.Select(
					dto => _entityFactory.CreateSection(
						dto.Id,
						dto.Title,
						sectionTypeLookup.GetValueOrDefault(dto.SectionTypeId)
						?? throw new InvalidOperationException(),
						letter,
						new List<Snippet>(),
						dto.SortOrder,
						_request.User
					)
				)
			, cancellationToken);
	}


	private async Task AddSnippetsAsync(
		Section sectionEntity,
		IEnumerable<Dtos.Snippet> dtos,
		CancellationToken cancellationToken
	)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		await _snippetRepository.AddRangeAsync(
			NewItems(sectionEntity.Snippets, dtos).Select(dto =>
				_entityFactory.CreateSnippet(
					dto.Id,
					dto.Title,
					sectionEntity,
					dto.SortOrder,
					_request.User
				)
			),
			cancellationToken
		);
	}


	private async Task AddKeyValuesAsync(
		Snippet snippetEntity,
		IEnumerable<Dtos.KeyValue> dtos,
		CancellationToken cancellationToken
	)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		await _keyValueRepository.AddRangeAsync(
			NewItems(snippetEntity.KeyValues, dtos)
				.Select(dto => _entityFactory.CreateKeyValue(
						dto.Id,
						snippetEntity,
						dto.SortOrder,
						dto.Value ?? "",
						dto.ValueType,
						dto.Key,
						dto.KeyType,
						_request.User
					)
				),
			cancellationToken
		);
	}

	private async Task UpdateSectionsAsync(
		IEnumerable<Section> entities,
		ICollection<Dtos.Section> source,
		CancellationToken cancellationToken
	)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		foreach ((Section Entity, Dtos.Section Dto) item in ExistingItems(entities, source))
		{
			if (item.Entity.Title == item.Dto.Title
			    && item.Entity.SortOrder == item.Dto.SortOrder
			    && item.Entity.SectionType.Id == item.Dto.SectionTypeId)
			{
				continue;
			}

			item.Entity.Title = item.Dto.Title;
			item.Entity.SortOrder = item.Dto.SortOrder;
			item.Entity.SectionType =
				await _sectionTypeRepository.GetByIdAsync(item.Dto.SectionTypeId, cancellationToken)
				?? throw new InvalidOperationException();

			item.Entity.Modified = _dateTimeProvider.Now;
			item.Entity.ModifiedBy = _request.User.UserName
			                         ?? throw new InvalidOperationException("Request is null");

			await _sectionRepository.SetAsync(item.Entity, cancellationToken);

			await HandleSnippetsAsync(item.Entity, item.Dto, cancellationToken);
		}
	}

	private static IList<(T Entity, TIncoming Dto)> ExistingItems<T, TIncoming>(IEnumerable<T> entities,
		ICollection<TIncoming> source)
		where T : IIdentifier<Guid>
		where TIncoming : IIdentifier<Guid>
	{
		return entities.Join(
			source,
			entity => entity.Id,
			dto => dto.Id,
			(entity, dto) => new { Entity = entity, Dto = dto }
		).Select(e => (e.Entity, e.Dto)).ToList();
	}

	private async Task UpdateSnippetsAsync(
		IEnumerable<Snippet> entities,
		ICollection<Dtos.Snippet> dtos,
		CancellationToken cancellationToken
	)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		foreach ((Snippet Entity, Dtos.Snippet Dto) item in ExistingItems(entities, dtos))
		{
			if (item.Entity.Title == item.Dto.Title
			    && item.Entity.SortOrder == item.Dto.SortOrder)
			{
				continue;
			}

			item.Entity.Title = item.Dto.Title;
			item.Entity.SortOrder = item.Dto.SortOrder;

			item.Entity.Modified = _dateTimeProvider.Now;
			item.Entity.ModifiedBy = _request.User.UserName!;

			await _snippetRepository.SetAsync(item.Entity, cancellationToken);

			await HandleKeyValuesAsync(item.Entity, item.Dto, cancellationToken);
		}
	}

	private async Task UpdateKeyValuesAsync(
		IEnumerable<KeyValue> entities,
		ICollection<Dtos.KeyValue> dtos,
		CancellationToken cancellationToken)
	{
		if (_request is null)
		{
			throw new InvalidOperationException("Request is null");
		}

		foreach ((KeyValue Entity, Dtos.KeyValue Dto) item in ExistingItems(entities, dtos))
		{
			if (item.Entity.Key == item.Dto.Key
			    && item.Entity.SortOrder == item.Dto.SortOrder
			    && item.Entity.KeyType == item.Dto.KeyType
			    && item.Entity.Value == item.Dto.Value
			    && item.Entity.ValueType == item.Dto.ValueType)
			{
				continue;
			}

			item.Entity.Value = item.Dto.Value ?? "";
			item.Entity.Key = item.Dto.Key;
			item.Entity.ValueType = item.Dto.ValueType;
			item.Entity.KeyType = item.Dto.KeyType;
			item.Entity.SortOrder = item.Dto.SortOrder;

			item.Entity.ModifiedBy = _request.User.UserName!;
			item.Entity.Modified = _dateTimeProvider.Now;

			await _keyValueRepository.SetAsync(item.Entity, cancellationToken);
		}
	}
}
