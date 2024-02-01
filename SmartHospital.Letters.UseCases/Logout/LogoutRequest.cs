using System.Security.Claims;
using MediatR;

namespace SmartHospital.Letters.UseCases.Logout;

public sealed record LogoutRequest(ClaimsPrincipal User) : IRequest<LogoutResponse>;
