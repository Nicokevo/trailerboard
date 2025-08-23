namespace TrailerBoard.Domain;

public class Movie
{
    public int Id { get; set; }                   
    public string PublicId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public int Year { get; set; }
    public string PosterUrl { get; set; } = default!;
    public string TrailerUrl { get; set; } = default!;
    public ICollection<Comment> Comments { get; set; } = [];
}