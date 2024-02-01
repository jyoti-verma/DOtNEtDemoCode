using MediatR;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;
using SmartHospital.Letters.Services.CreateSnippets;
using Snippet = SmartHospital.Letters.Dtos.Snippet;

namespace SmartHospital.Letters.UseCases.GetSnippets;

public sealed class GetSnippetsHandler : IRequestHandler<GetSnippetsRequest, GetSnippetsResponse>
{
	private readonly ICreateSnippetsDispatcher _createSnippetsDispatcher;
	private readonly ISectionRepository _sectionRepository;

	public GetSnippetsHandler(
		ISectionRepository sectionRepository,
		ICreateSnippetsDispatcher createSnippetsDispatcher
	)
	{
		_sectionRepository = sectionRepository;
		_createSnippetsDispatcher = createSnippetsDispatcher;
	}

	public async Task<GetSnippetsResponse> Handle(GetSnippetsRequest request,
		CancellationToken cancellationToken = default)
	{
		Section? section = await _sectionRepository.GetByIdAsync(request.SectionId, cancellationToken);
		if (section is null)
		{
			return new GetSnippetsResponse(
				(int)Codes.SectionDoesNotExists,
				$"Section with {request.SectionId} does not exists"
			);
		}

		IEnumerable<Snippet> snippets = await _createSnippetsDispatcher.ExecuteAsync(
			section.SectionType.Name,
			section.Letter.ExternalPatientId,
			section.Letter.ExternalCaseNumber,
			cancellationToken);

		IEnumerable<Snippet> enumerable = snippets as Snippet[] ?? snippets.ToArray();
		return enumerable.Any()
			? new GetSnippetsResponse(enumerable.ToList())
			: new GetSnippetsResponse((int)Codes.SnippetsCreateFailed, "No snippets available");
	}
}
