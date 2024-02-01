namespace SmartHospital.Letters.Repositories;

public interface ILetterRepository : IRepository<Entities.Letter>
{
	new IQueryable<Entities.Letter> All();
	new Task<Entities.Letter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<ICollection<Entities.Letter>> AllOrderedBySectionSortOrderAsync(CancellationToken cancellationToken = default);
}
