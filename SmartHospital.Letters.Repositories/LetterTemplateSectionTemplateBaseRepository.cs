using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Repositories;

public class LetterTemplateSectionTemplateBaseRepository : BaseRepository<LetterTemplateSectionTemplate>,
	ILetterTemplateSectionTemplateRepository
{
	public LetterTemplateSectionTemplateBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public async Task<LetterTemplateSectionTemplate?> GetByAsync(Guid letterTemplateId, Guid sectionTemplateId,
		CancellationToken cancellationToken)
	{
		return await All()
			.SingleOrDefaultAsync(p => p.LetterTemplateId == letterTemplateId
			                           && p.SectionTemplateId == sectionTemplateId, cancellationToken);
	}
}
