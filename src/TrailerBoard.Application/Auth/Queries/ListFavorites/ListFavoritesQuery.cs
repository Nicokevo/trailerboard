using MediatR;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Favorites.Queries.ListFavorites;

public sealed record ListFavoritesQuery(string Email) : IRequest<IReadOnlyList<FavoriteDto>>;
