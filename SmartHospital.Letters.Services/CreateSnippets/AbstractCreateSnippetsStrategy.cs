using SmartHospital.Letters.Dtos;
using SmartHospital.Letters.Fhir.Api.Client;

namespace SmartHospital.Letters.Services.CreateSnippets;

/// <summary>
///     Abstract class for creating snippets.
/// </summary>
public abstract class AbstractCreateSnippetsStrategy : ICreateSnippetsStrategy
{
	protected readonly IFhirApiClient FhirApiClient;

	protected AbstractCreateSnippetsStrategy(IFhirApiClient fhirApiClient)
	{
		FhirApiClient = fhirApiClient;
	}

	/// <summary>
	///     Should return a list of snippets, based on the given parameters and the strategy.
	/// </summary>
	/// <param name="externalPatientId"></param>
	/// <param name="externalCaseNumber"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public abstract Task<IEnumerable<Snippet>> CreateAsync(
		string externalPatientId,
		string externalCaseNumber,
		CancellationToken cancellationToken
	);
}
