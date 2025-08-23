namespace TrailerBoard.Application;

public interface IAppDbContext
{
    IQueryable<TrailerBoard.Domain.Movie> Movies { get; }
    IQueryable<TrailerBoard.Domain.Favorite> Favorites { get; }
    IQueryable<TrailerBoard.Domain.Comment> Comments { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}