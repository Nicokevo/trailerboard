using MediatR;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Favorites.Queries.ListFavorites;

public sealed class ListFavoritesHandler
    : IRequestHandler<ListFavoritesQuery, IReadOnlyList<FavoriteDto>>
{
    private readonly IFavoriteRepository _favorites;

    public ListFavoritesHandler(IFavoriteRepository favorites)
    {
        _favorites = favorites;
    }

    public async Task<IReadOnlyList<FavoriteDto>> Handle(ListFavoritesQuery request, CancellationToken ct)
    {
        var list = await _favorites.ListByUserAsync(request.Email, ct);

        return list.Select(f => new FavoriteDto(
            f.Movie.PublicId,
            f.Movie.Title,
            f.Movie.Year,
            f.Movie.PosterUrl,
            f.Movie.TrailerUrl
        )).ToList();
    }
}
