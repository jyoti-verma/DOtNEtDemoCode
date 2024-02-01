using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public interface ISectionTypeRepository : IRepository<SectionType>
{
	Task<SectionType> GetByDefaultTitleAsync(string name, CancellationToken cancellationToken = default);
}
