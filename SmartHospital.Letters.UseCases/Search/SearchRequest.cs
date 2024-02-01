using MediatR;

namespace SmartHospital.Letters.UseCases.Search;

public sealed record SearchRequest(string Text) : IRequest<SearchResponse>;
