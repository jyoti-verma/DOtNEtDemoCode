using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public interface ILetterTypeRepository : IRepository<LetterType>
{
	Task<LetterType?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
