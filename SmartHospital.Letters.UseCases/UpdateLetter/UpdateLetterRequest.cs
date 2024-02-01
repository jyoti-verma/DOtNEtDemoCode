using MediatR;
using SmartHospital.Letters.Domain;

namespace SmartHospital.Letters.UseCases.UpdateLetter;

public sealed record UpdateLetterRequest(Dtos.Letter Letter, LetterUser User) : IRequest<UpdateLetterResponse>;
