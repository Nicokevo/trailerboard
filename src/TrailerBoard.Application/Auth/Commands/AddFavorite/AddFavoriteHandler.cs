using MediatR;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Contracts;
using TrailerBoard.Domain;

namespace TrailerBoard.Application.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteHandler
    : IRequestHandler<AddFavoriteCommand, FavoriteDto>
{
    private readonly IMovieRepository _movies;
    private readonly IFavoriteRepository _favorites;
    private readonly IUnitOfWork _uow;

    public AddFavoriteHandler(IMovieRepository movies, IFavoriteRepository favorites, IUnitOfWork uow)
    {
        _movies = movies;
        _favorites = favorites;
        _uow = uow;
    }

    public async Task<FavoriteDto> Handle(AddFavoriteCommand request, CancellationToken ct)
    {
        var movie = await _movies.GetByPublicIdAsync(request.PublicId, ct);
        if (movie is null)
            throw new KeyNotFoundException("movie_not_found");

        if (await _favorites.ExistsAsync(request.Email, movie.Id, ct))
            throw new InvalidOperationException("favorite_exists");

        var favorite = new Favorite { UserEmail = request.Email, MovieId = movie.Id };
        await _favorites.AddAsync(favorite, ct);
        await _uow.SaveChangesAsync(ct);

        return new FavoriteDto(movie.PublicId, movie.Title, movie.Year, movie.PosterUrl, movie.TrailerUrl);
    }
}
