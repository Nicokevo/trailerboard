using TrailerBoard.Domain;

namespace TrailerBoard.Application.Abstractions.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
}
