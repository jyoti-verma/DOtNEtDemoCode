using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.Services.CreateSnippets;

public interface ICreateSnippetsDispatcher
{
	Task<IEnumerable<Snippet>> ExecuteAsync(
		string sectionTypeName,
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	);
}
