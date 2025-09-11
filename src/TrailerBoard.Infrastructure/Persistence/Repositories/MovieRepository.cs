using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Domain;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Persistence.Repositories;

public sealed class MovieRepository : GenericRepository<Movie>, IMovieRepository
{
    public MovieRepository(TrailerDbContext db) : base(db) { }

    public Task<Movie?> GetByPublicIdAsync(string publicId, CancellationToken ct = default) =>
        _db.Movies.FirstOrDefaultAsync(m => m.PublicId == publicId, ct);
}
