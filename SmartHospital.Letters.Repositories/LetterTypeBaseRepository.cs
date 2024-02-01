using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public class LetterTypeBaseRepository : BaseRepository<LetterType>, ILetterTypeRepository
{
	public LetterTypeBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public async Task<LetterType?> GetByNameAsync(string name, CancellationToken cancellationToken)
	{
		return await All()
			.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
	}
}
