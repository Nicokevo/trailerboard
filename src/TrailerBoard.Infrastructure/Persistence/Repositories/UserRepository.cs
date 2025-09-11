using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Domain;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Persistence.Repositories;

public sealed class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(TrailerDbContext db) : base(db) { }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct = default) =>
        _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
}
