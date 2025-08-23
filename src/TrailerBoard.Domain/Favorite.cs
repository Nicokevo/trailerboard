namespace TrailerBoard.Domain;

public class Favorite
{
    public int Id { get; set; }
    public string UserEmail { get; set; } = default!;
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = default!;
}