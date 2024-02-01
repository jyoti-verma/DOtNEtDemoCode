using MediatR;
using SmartHospital.Letters.Repositories;

namespace SmartHospital.Letters.UseCases.GetLetters;

public sealed class GetLettersHandler : IRequestHandler<GetLettersRequest, GetLettersResponse>
{
	private readonly ILetterRepository _letterRepository;

	public GetLettersHandler(ILetterRepository letterRepository)
	{
		_letterRepository = letterRepository;
	}

	public async Task<GetLettersResponse> Handle(GetLettersRequest request,
		CancellationToken cancellationToken = default)
	{
		IEnumerable<Entities.Letter> letters = await _letterRepository
			.AllOrderedBySectionSortOrderAsync(cancellationToken);

		return letters.Any()
			? new GetLettersResponse(letters
				.Select(Dtos.Letter.FromLetterEntity)
				.ToList())
			: new GetLettersResponse((int)Codes.NoLettersFound, "nothing found");
	}
}
