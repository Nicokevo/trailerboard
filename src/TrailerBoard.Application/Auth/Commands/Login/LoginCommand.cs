using MediatR;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<AuthResponse>;
