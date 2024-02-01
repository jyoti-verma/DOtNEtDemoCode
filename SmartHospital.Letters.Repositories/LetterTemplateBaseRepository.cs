using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Repositories;

public sealed class LetterTemplateBaseRepository : BaseRepository<LetterTemplate>, ILetterTemplateRepository
{
	public LetterTemplateBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public async Task<LetterTemplate?> GetLetterTemplateAsync(LetterType letterType,
		CancellationToken cancellationToken)
	{
		return await All()
			.Include(p => p.SectionTemplates)
			.ThenInclude(p => p.SnippetTemplates)
			.SingleOrDefaultAsync(
				p => p.LetterType == letterType,
				cancellationToken
			);
	}

	public async Task<LetterTemplate?> GetByLetterTypeNameAsync(string letterTypeName,
		CancellationToken cancellationToken)
	{
		return await All()
			.Include(p => p.LetterType)
			.Include(p => p.SectionTemplates)
			.ThenInclude(p => p.SectionType)
			.Include(p => p.SectionTemplates)
			.ThenInclude(p => p.SnippetTemplates)
			.SingleOrDefaultAsync(
				p => p.LetterType.Name == letterTypeName,
				cancellationToken
			);
	}
}
