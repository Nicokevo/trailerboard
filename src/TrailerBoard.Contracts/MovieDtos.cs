namespace TrailerBoard.Contracts;

public record CreateMovieRequest(
    string PublicId,
    string Title,
    int Year,
    string PosterUrl,
    string TrailerUrl
);

public record MovieDtos(
    string PublicId,
    string Title,
    int Year,
    string PosterUrl,
    string TrailerUrl
);
