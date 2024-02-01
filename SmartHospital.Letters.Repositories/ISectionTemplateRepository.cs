using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public interface ISectionTemplateRepository : IRepository<SectionTemplate>
{
	Task<SectionTemplate?> GetBySectionNameAsync(string sectionName, CancellationToken cancellationToken = default);
}
