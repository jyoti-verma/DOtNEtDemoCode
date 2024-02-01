using MediatR;

namespace SmartHospital.Letters.UseCases.GetLetters;

public sealed record GetLettersRequest : IRequest<GetLettersResponse>;
