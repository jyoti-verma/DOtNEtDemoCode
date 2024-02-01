using MediatR;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.UseCases.CreateLetterFromExisting;

public sealed record CreateLetterFromExistingRequest(
	Guid Id,
	string ExternalCaseNumber,
	LetterUser User
) : IRequest<CreateLetterFromExistingResponse>;
