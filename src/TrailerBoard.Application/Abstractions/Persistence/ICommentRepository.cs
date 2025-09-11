using TrailerBoard.Domain;

namespace TrailerBoard.Application.Abstractions.Persistence;

public interface ICommentRepository : IGenericRepository<Comment>
{
    Task<IReadOnlyList<Comment>> ListByMovieAsync(int movieId, CancellationToken ct = default);
}
