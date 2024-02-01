using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Entities.Templates;

namespace SmartHospital.Letters.Repositories;

public interface ILetterTemplateRepository : IRepository<LetterTemplate>
{
	Task<LetterTemplate?> GetLetterTemplateAsync(LetterType letterType, CancellationToken cancellationToken = default);

	Task<LetterTemplate?>
		GetByLetterTypeNameAsync(string letterTypeName, CancellationToken cancellationToken = default);
}
