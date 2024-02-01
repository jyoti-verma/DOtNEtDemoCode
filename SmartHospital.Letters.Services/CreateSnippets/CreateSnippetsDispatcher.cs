using SmartHospital.Letters.Entities;
using Snippet = SmartHospital.Letters.Dtos.Snippet;

namespace SmartHospital.Letters.Services.CreateSnippets;

public sealed class CreateSnippetsDispatcher : ICreateSnippetsDispatcher
{
	private readonly IEnumerable<ICreateSnippetsStrategy> _createSnippetsStrategies;

	public CreateSnippetsDispatcher(
		IEnumerable<ICreateSnippetsStrategy> createSnippetsStrategies
	)
	{
		_createSnippetsStrategies = createSnippetsStrategies;
	}

	public async Task<IEnumerable<Snippet>> ExecuteAsync(
		string sectionTypeName,
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	)
	{
		if (!Enum.TryParse(typeof(SectionTypeNames), sectionTypeName, out _))
		{
			throw new NotImplementedException($"The given {sectionTypeName} is not implemented.");
		}

		ICreateSnippetsStrategy? strategy = _createSnippetsStrategies
			.FirstOrDefault(p => p.GetType().Name == sectionTypeName + "CreateSnippetsStrategy");

		return strategy is not null
			? await strategy.CreateAsync(externalPatientId, externalCaseNumber, cancellationToken)
			: new List<Snippet>();
	}
}
