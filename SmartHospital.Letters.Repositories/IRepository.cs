using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public interface IRepository<T> where T : BaseClass
{
	Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task AddAsync(T entity, CancellationToken cancellationToken = default);
	Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
	Task SetAsync(T entity, CancellationToken cancellationToken = default);
	void Remove(T entity);
	Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);
	Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
	Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	IQueryable<T> All();
}
