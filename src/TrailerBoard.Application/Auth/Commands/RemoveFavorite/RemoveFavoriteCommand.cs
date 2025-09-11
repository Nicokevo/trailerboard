using MediatR;

namespace TrailerBoard.Application.Favorites.Commands.RemoveFavorite;

public sealed record RemoveFavoriteCommand(string Email, string PublicId) : IRequest<Unit>;