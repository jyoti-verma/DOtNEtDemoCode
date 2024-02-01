using MediatR;
using SmartHospital.Letters.Dtos;

namespace SmartHospital.Letters.UseCases.RefreshToken;

public sealed record RefreshTokenRequest(TokenResponse TokenResponse) : IRequest<RefreshTokenResponse>;
