using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Favorites;
using TrailerBoard.Contracts;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Favorites;

public sealed class FavoritesService : IFavoritesService
{
    private readonly TrailerDbContext _db;
    public FavoritesService(TrailerDbContext db) => _db = db;

    public async Task<FavoriteDto> AddAsync(string email, string publicId, CancellationToken ct = default)
    {
        var movie = await _db.Movies.FirstOrDefaultAsync(m => m.PublicId == publicId, ct);
        if (movie is null) throw new KeyNotFoundException("movie_not_found");

        var exists = await _db.Favorites.AnyAsync(f => f.UserEmail == email && f.MovieId == movie.Id, ct);
        if (exists) throw new InvalidOperationException("favorite_exists");

        _db.Favorites.Add(new Domain.Favorite { UserEmail = email, MovieId = movie.Id });
        await _db.SaveChangesAsync(ct);

        return new FavoriteDto(movie.PublicId, movie.Title, movie.Year, movie.PosterUrl, movie.TrailerUrl);
    }

    public async Task<IReadOnlyList<FavoriteDto>> ListAsync(string email, CancellationToken ct = default)
    {
        return await _db.Favorites
            .Where(f => f.UserEmail == email)
            .OrderBy(f => f.Movie.Title)
            .Select(f => new FavoriteDto(
                f.Movie.PublicId, f.Movie.Title, f.Movie.Year, f.Movie.PosterUrl, f.Movie.TrailerUrl))
            .ToListAsync(ct);
    }
}
