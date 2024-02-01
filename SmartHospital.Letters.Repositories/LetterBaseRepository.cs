using Microsoft.EntityFrameworkCore;
using SmartHospital.Letters.Context;
using SmartHospital.Letters.Core;
using SmartHospital.Letters.Entities;

namespace SmartHospital.Letters.Repositories;

public sealed class LetterBaseRepository : BaseRepository<Entities.Letter>, ILetterRepository
{
	public LetterBaseRepository(LetterDbContext dbContext, IDateTimeProvider dateTimeProvider)
		: base(dbContext, dateTimeProvider)
	{
	}

	public new IQueryable<Entities.Letter> All()
	{
		return base.All()
			.Include(p => p.LetterType)
			.Include(p => p.Sections)
			.ThenInclude(p => p.SectionType)
			.Include(p => p.Sections)
			.ThenInclude(p => p.Snippets)
			.ThenInclude(p => p.KeyValues)
			.OrderByDescending(p => p.Modified > p.Created ? p.Modified : p.Created);
	}

	public new async Task<Entities.Letter?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		Entities.Letter? letter = await All()
			.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

		if (letter is null)
		{
			return letter;
		}

		OrderSectionSnippetAndKeyValues(letter);

		return letter;
	}

	public async Task<ICollection<Entities.Letter>> AllOrderedBySectionSortOrderAsync(
		CancellationToken cancellationToken = default)
	{
		var result = new List<Entities.Letter>();
		foreach (Entities.Letter letter in await All().ToListAsync(cancellationToken))
		{
			OrderSectionSnippetAndKeyValues(letter);
			result.Add(letter);
		}

		return result;
	}


	private static void OrderSectionSnippetAndKeyValues(Entities.Letter letter)
	{
		letter.Sections =
			new List<Section>(
				letter.Sections.OrderBy(p => p.SortOrder == 0 ? int.MaxValue : p.SortOrder)
			);

		int sectionOrder = 1;
		foreach (Section section in letter.Sections)
		{
			section.SortOrder = sectionOrder++;
			section.Snippets =
				new List<Snippet>(section.Snippets.OrderBy(s => s.SortOrder == 0 ? int.MaxValue : s.SortOrder));

			int snippetOrder = 1;
			foreach (Snippet snippet in section.Snippets)
			{
				snippet.SortOrder = snippetOrder++;
				snippet.KeyValues =
					new List<KeyValue>(snippet.KeyValues.OrderBy(k => k.SortOrder == 0 ? int.MaxValue : k.SortOrder));

				int keyValueOrder = 1;
				foreach (KeyValue keyValue in snippet.KeyValues)
				{
					keyValue.SortOrder = keyValueOrder++;
				}
			}
		}
	}
}
