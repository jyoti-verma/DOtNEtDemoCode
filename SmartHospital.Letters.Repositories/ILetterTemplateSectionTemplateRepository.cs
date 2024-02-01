using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Repositories;

public interface ILetterTemplateSectionTemplateRepository : IRepository<LetterTemplateSectionTemplate>
{
	Task<LetterTemplateSectionTemplate?> GetByAsync(Guid letterTemplateId, Guid sectionTemplateId,
		CancellationToken cancellationToken = default);
}
