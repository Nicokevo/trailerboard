using MediatR;
using TrailerBoard.Contracts;

namespace TrailerBoard.Application.Favorites.Commands.AddFavorite;

public sealed record AddFavoriteCommand(string Email, string PublicId)
    : IRequest<FavoriteDto>;
