using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Domain;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Persistence.Repositories;

public sealed class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(TrailerDbContext db) : base(db) { }

    public async Task<bool> ExistsAsync(string email, int movieId, CancellationToken ct = default)
        => await _db.Favorites.AnyAsync(f => f.UserEmail == email && f.MovieId == movieId, ct);

    public async Task<IReadOnlyList<Favorite>> ListByUserAsync(string email, CancellationToken ct = default)
        => await _db.Favorites
            .Include(f => f.Movie)
            .Where(f => f.UserEmail == email)
            .ToListAsync(ct);

    public async Task<Favorite?> GetByUserAndMovieAsync(string email, int movieId, CancellationToken ct = default)
        => await _db.Favorites.FirstOrDefaultAsync(f => f.UserEmail == email && f.MovieId == movieId, ct);

    public async Task<bool> DeleteByUserAndMovieIdAsync(string email, int movieId, CancellationToken ct = default)
    {

        var affected = await _db.Favorites
            .Where(f => f.UserEmail == email && f.MovieId == movieId)
            .ExecuteDeleteAsync(ct);   

        return affected > 0;

    }
}
