using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly TrailerDbContext _db;
    public UnitOfWork(TrailerDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);
}
