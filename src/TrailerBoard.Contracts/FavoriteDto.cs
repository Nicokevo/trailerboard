namespace TrailerBoard.Contracts;

public record AddFavoriteRequest(string PublicId);

public record FavoriteDto(
    string PublicId,
    string Title,
    int Year,
    string PosterUrl,
    string TrailerUrl
);
