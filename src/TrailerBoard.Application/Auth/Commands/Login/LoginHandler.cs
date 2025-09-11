using MediatR;
using BCrypt.Net;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Application.Abstractions.Security;
using TrailerBoard.Contracts;
using TrailerBoard.Domain;

namespace TrailerBoard.Application.Auth.Commands.Login;

public sealed class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _users;
    private readonly ITokenGenerator _tokenGenerator;

    public LoginHandler(IUserRepository users, ITokenGenerator tokenGenerator)
    {
        _users = users;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _users.GetByEmailAsync(request.Email, ct);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("invalid_credentials");

        var token = _tokenGenerator.Generate(user);
        return new AuthResponse(token, user.Email, user.FirstName, user.LastName);
    }
}
