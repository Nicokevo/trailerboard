using TrailerBoard.Domain;

namespace TrailerBoard.Application.Abstractions.Persistence;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<bool> ExistsAsync(string email, int movieId, CancellationToken ct = default);
    Task<IReadOnlyList<Favorite>> ListByUserAsync(string email, CancellationToken ct = default);


    Task<Favorite?> GetByUserAndMovieAsync(string email, int movieId, CancellationToken ct = default);

    Task<bool> DeleteByUserAndMovieIdAsync(string email, int movieId, CancellationToken ct = default);
}
