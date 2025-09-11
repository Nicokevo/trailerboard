using MediatR;
using TrailerBoard.Contracts;
using System.Security.Claims;

namespace TrailerBoard.Application.Auth.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery(ClaimsPrincipal User) : IRequest<AuthResponse>;
