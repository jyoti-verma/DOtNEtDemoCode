using MediatR;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.Repositories;

namespace SmartHospital.Letters.UseCases.CreateLetterFromExisting;

public sealed class
	CreateLetterFromExistingHandler : IRequestHandler<CreateLetterFromExistingRequest, CreateLetterFromExistingResponse>
{
	private readonly IEntityFactory _entityFactory;
	private readonly ILetterRepository _letterRepository;

	public CreateLetterFromExistingHandler(
		IEntityFactory entityFactory,
		ILetterRepository letterRepository
	)
	{
		_entityFactory = entityFactory;
		_letterRepository = letterRepository;
	}

	public async Task<CreateLetterFromExistingResponse> Handle(
		CreateLetterFromExistingRequest request,
		CancellationToken cancellationToken = default
	)
	{
		Entities.Letter? letter = await _letterRepository.GetByIdAsync(request.Id, cancellationToken);
		if (letter is null)
		{
			return new CreateLetterFromExistingResponse(
				(int)Codes.LetterDoesNotExists,
				$"letter with {request.Id} not found"
			);
		}

		Entities.Letter newLetter = _entityFactory.CloneLetter(
			letter,
			request.ExternalCaseNumber,
			request.User
		);

		await _letterRepository.InsertAsync(newLetter, cancellationToken);

		return new CreateLetterFromExistingResponse(
			Dtos.Letter.FromLetterEntity(newLetter)
		);
	}
}
