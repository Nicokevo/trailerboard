using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Abstractions.Persistence;

namespace TrailerBoard.Infrastructure.Persistence.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly TrailerDbContext _db;

    public GenericRepository(TrailerDbContext db)
    {
        _db = db;
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken ct = default)
        => await _db.Set<T>().FindAsync(new object[] { id }, ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _db.Set<T>().AddAsync(entity, ct);

    public Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        _db.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        _db.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public IQueryable<T> Query() // 👈 implementación
        => _db.Set<T>().AsQueryable();
}
