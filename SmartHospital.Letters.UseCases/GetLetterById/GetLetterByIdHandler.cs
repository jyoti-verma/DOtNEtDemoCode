using MediatR;
using SmartHospital.Letters.Repositories;

namespace SmartHospital.Letters.UseCases.GetLetterById;

public sealed class GetLetterByIdHandler : IRequestHandler<GetLetterByIdRequest, GetLetterByIdResponse>
{
	private readonly ILetterRepository _letterRepository;

	public GetLetterByIdHandler(ILetterRepository letterRepository)
	{
		_letterRepository = letterRepository;
	}

	public async Task<GetLetterByIdResponse> Handle(
		GetLetterByIdRequest request,
		CancellationToken cancellationToken
	)
	{
		Entities.Letter? letter = await _letterRepository.GetByIdAsync(request.Id, cancellationToken);
		return letter is null
			? new GetLetterByIdResponse(
				(int)Codes.LetterDoesNotExists,
				$"Letter with {request.Id} not found")
			: new GetLetterByIdResponse(
				Dtos.Letter.FromLetterEntity(letter)
			);
	}
}
