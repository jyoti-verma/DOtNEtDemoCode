using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public class SectionBaseRepository : BaseRepository<Section>, ISectionRepository
{
	public SectionBaseRepository(
		LetterDbContext dbContext,
		IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public new async Task<Section?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		return await All()
			.Include(p => p.SectionType)
			.Include(p => p.Letter)
			.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
	}
}
