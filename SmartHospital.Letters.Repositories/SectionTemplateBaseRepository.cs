using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public class SectionTemplateBaseRepository : BaseRepository<SectionTemplate>, ISectionTemplateRepository
{
	public SectionTemplateBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public async Task<SectionTemplate?> GetBySectionNameAsync(string sectionName, CancellationToken cancellationToken)
	{
		return await All()
			.Include(p => p.SectionType)
			.SingleOrDefaultAsync(p => p.SectionType.DefaultTitle == sectionName, cancellationToken);
	}
}
