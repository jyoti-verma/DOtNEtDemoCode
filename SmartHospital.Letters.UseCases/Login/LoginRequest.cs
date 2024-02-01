using System.Security.Claims;
using MediatR;

namespace SmartHospital.Letters.UseCases.Login;

public sealed record LoginRequest(ClaimsPrincipal User) : IRequest<LoginResponse>;
