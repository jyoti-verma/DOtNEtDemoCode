using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.DefaultValues;

namespace SmartHospital.Letters.Services.Extensions;

public static class RepositoryExtensions
{
	public static async Task<bool> TryAddAsync<T>(
		this IRepository<T> repo,
		T entity,
		CancellationToken cancellationToken = default
	) where T : BaseClass
	{
		if (await repo.GetByIdAsync(entity.Id, cancellationToken) is null)
		{
			await repo.InsertAsync(entity, cancellationToken);
			return true;
		}

		return false;
	}

	public static async Task<SectionTemplate> GetBySectionNameOrThrowAsync(
		this ISectionTemplateRepository repo,
		string name,
		CancellationToken cancellationToken = default
	)
	{
		return await repo.GetBySectionNameAsync(name, cancellationToken)
		       ?? throw new SectionNotFoundException(name);
	}

	public static async Task<SectionType> GetByNameOrThrowAsync(
		this ISectionTypeRepository repo,
		string name,
		CancellationToken cancellationToken = default
	)
	{
		return await repo.GetByDefaultTitleAsync(name, cancellationToken)
		       ?? throw new SectionNotFoundException(name);
	}

	public static async Task<LetterType> GetByNameOrThrowAsync(
		this ILetterTypeRepository repo,
		string name,
		CancellationToken cancellationToken = default
	)
	{
		return await repo.GetByNameAsync(name, cancellationToken)
		       ?? throw new LetterTypeNotFoundException(name);
	}
}
