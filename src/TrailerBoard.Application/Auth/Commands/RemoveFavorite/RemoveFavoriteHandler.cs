using MediatR;
using TrailerBoard.Application.Abstractions.Persistence;

namespace TrailerBoard.Application.Favorites.Commands.RemoveFavorite;

public sealed class RemoveFavoriteHandler : IRequestHandler<RemoveFavoriteCommand, Unit>
{
    private readonly IMovieRepository _movies;
    private readonly IFavoriteRepository _favorites;
    private readonly IUnitOfWork _uow;

    public RemoveFavoriteHandler(IMovieRepository movies, IFavoriteRepository favorites, IUnitOfWork uow)
    {
        _movies = movies;
        _favorites = favorites;
        _uow = uow;
    }

    public async Task<Unit> Handle(RemoveFavoriteCommand request, CancellationToken ct)
    {
        var movie = await _movies.GetByPublicIdAsync(request.PublicId, ct);
        if (movie is null) throw new KeyNotFoundException("movie_not_found");

        var deleted = await _favorites.DeleteByUserAndMovieIdAsync(request.Email, movie.Id, ct);
        if (!deleted) throw new KeyNotFoundException("favorite_not_found");

        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
