using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public class BaseRepository<T> : IRepository<T>
	where T : BaseClass

{
	private readonly IDateTimeProvider _dateTimeProvider;
	protected readonly LetterDbContext DbContext;

	public BaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
	{
		DbContext = dbContext;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await DbContext.Set<T>()
			.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
	}

	public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
	{
		await DbContext.Set<T>().AddAsync(entity, cancellationToken);
	}

	public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
	{
		await DbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
	}

	public async Task SetAsync(T entity, CancellationToken cancellationToken = default)
	{
		T? currentEntity = await DbContext.FindAsync<T>(entity.Id, cancellationToken);
		if (currentEntity is not null)
		{
			DbContext.Entry(currentEntity).CurrentValues.SetValues(entity);
		}
	}

	public void Remove(T entity)
	{
		DbContext.Set<T>().Remove(entity);
	}

	public async Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default)
	{
		await AddAsync(entity, cancellationToken);
		return await SaveChangesAsync(cancellationToken);
	}

	public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
	{
		await SetAsync(entity, cancellationToken);
		return await SaveChangesAsync(cancellationToken);
	}

	public async Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
	{
		DbContext.Set<T>().Remove(entity);
		return await SaveChangesAsync(cancellationToken);
	}

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return await DbContext.SaveChangesAsync(cancellationToken);
	}

	public IQueryable<T> All()
	{
		return DbContext.Set<T>();
	}
}
