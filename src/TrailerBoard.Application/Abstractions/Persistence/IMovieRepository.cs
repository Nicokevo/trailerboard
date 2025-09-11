using TrailerBoard.Domain;

namespace TrailerBoard.Application.Abstractions.Persistence;

public interface IMovieRepository
{
    Task<Movie?> GetByPublicIdAsync(string publicId, CancellationToken ct = default);
}
