using System.Linq.Expressions;

namespace TrailerBoard.Application.Abstractions.Persistence;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task UpdateAsync(T entity, CancellationToken ct = default);
    Task DeleteAsync(T entity, CancellationToken ct = default);

    IQueryable<T> Query(); 
}
