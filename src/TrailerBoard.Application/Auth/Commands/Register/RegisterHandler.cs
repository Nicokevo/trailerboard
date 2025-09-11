using MediatR;
using BCrypt.Net;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Application.Abstractions.Security;
using TrailerBoard.Contracts;
using TrailerBoard.Domain;

namespace TrailerBoard.Application.Auth.Commands.Register;

public sealed class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;
    private readonly ITokenGenerator _tokenGenerator;

    public RegisterHandler(IUserRepository users, IUnitOfWork uow, ITokenGenerator tokenGenerator)
    {
        _users = users;
        _uow = uow;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (request.Password != request.ConfirmPassword)
            throw new InvalidOperationException("password_mismatch");

        var exists = await _users.GetByEmailAsync(request.Email, ct);
        if (exists is not null)
            throw new InvalidOperationException("email_exists");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        await _users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct); 

        var token = _tokenGenerator.Generate(user);
        return new AuthResponse(token, user.Email, user.FirstName, user.LastName);
    }
}
