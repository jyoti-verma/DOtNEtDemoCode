using MediatR;

namespace SmartHospital.Letters.UseCases.GetSnippets;

public sealed record GetSnippetsRequest(Guid SectionId) : IRequest<GetSnippetsResponse>;
