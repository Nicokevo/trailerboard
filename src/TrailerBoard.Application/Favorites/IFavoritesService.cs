using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Favorites;

public interface IFavoritesService
{
    Task<FavoriteDto> AddAsync(string userEmail, string publicId, CancellationToken ct = default);
    Task<IReadOnlyList<FavoriteDto>> ListAsync(string userEmail, CancellationToken ct = default);
}
