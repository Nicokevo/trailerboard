using MediatR;
using TrailerBoard.Contracts;
using TrailerBoard.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TrailerBoard.Application.Auth.Queries.GetCurrentUser;

public sealed class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, AuthResponse>
{
    private readonly IUserRepository _users;

    public GetCurrentUserHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<AuthResponse> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        var email = request.User.FindFirst(ClaimTypes.Email)?.Value
            ?? throw new UnauthorizedAccessException("no_email");

        var user = await _users.Query()
            .Where(u => u.Email == email)
            .Select(u => new AuthResponse("", u.Email, u.FirstName, u.LastName))
            .FirstOrDefaultAsync(ct);

        return user is null ? throw new KeyNotFoundException("user_not_found") : user;
    }
}
