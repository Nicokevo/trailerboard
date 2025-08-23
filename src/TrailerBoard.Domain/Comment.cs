namespace TrailerBoard.Domain;

public class Comment
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; } = default!;
    public string UserEmail { get; set; } = default!;
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}