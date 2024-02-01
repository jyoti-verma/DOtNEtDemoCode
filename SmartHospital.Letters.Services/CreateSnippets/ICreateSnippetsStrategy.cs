using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public interface ICreateSnippetsStrategy
{
	Task<IEnumerable<Snippet>> CreateAsync(
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken = default
	);
}
