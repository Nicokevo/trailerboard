using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Domain;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure.Persistence.Repositories;

public sealed class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
    public CommentRepository(TrailerDbContext db) : base(db) { }

    public async Task<IReadOnlyList<Comment>> ListByMovieAsync(int movieId, CancellationToken ct = default)
    {
        return await _db.Comments
            .Where(c => c.MovieId == movieId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}
