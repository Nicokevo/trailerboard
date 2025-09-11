using MediatR;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Auth.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName
) : IRequest<AuthResponse>;
