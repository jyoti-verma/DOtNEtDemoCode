using MediatR;

namespace SmartHospital.Letters.UseCases.GetLetterById;

public sealed record GetLetterByIdRequest(Guid Id) : IRequest<GetLetterByIdResponse>;
