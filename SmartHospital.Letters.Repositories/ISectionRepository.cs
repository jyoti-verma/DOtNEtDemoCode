using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public interface ISectionRepository : IRepository<Section>
{
	new Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
