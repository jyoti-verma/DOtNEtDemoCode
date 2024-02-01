using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public class SectionTypeBaseRepository : BaseRepository<SectionType>, ISectionTypeRepository
{
	public SectionTypeBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public async Task<SectionType> GetByDefaultTitleAsync(string name, CancellationToken cancellationToken)
	{
		return await All().SingleAsync(p => p.DefaultTitle == name, cancellationToken);
	}
}
