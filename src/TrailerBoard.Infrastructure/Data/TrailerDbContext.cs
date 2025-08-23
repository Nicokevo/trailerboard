using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application;
using TrailerBoard.Domain;

namespace TrailerBoard.Infrastructure.Data;

public class TrailerDbContext : DbContext, IAppDbContext
{
    public TrailerDbContext(DbContextOptions<TrailerDbContext> options) : base(options) {}

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<Comment> Comments => Set<Comment>();

    IQueryable<Movie> IAppDbContext.Movies => Movies.AsQueryable();
    IQueryable<Favorite> IAppDbContext.Favorites => Favorites.AsQueryable();
    IQueryable<Comment> IAppDbContext.Comments => Comments.AsQueryable();
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
       => base.SaveChangesAsync(ct);
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Movie>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.PublicId).HasMaxLength(64).IsRequired();
            e.HasIndex(x => x.PublicId).IsUnique();
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.Property(x => x.PosterUrl).HasMaxLength(500).IsRequired();
            e.Property(x => x.TrailerUrl).HasMaxLength(500).IsRequired();
        });

        b.Entity<Favorite>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UserEmail).HasMaxLength(200).IsRequired();
            e.HasIndex(x => new { x.UserEmail, x.MovieId }).IsUnique();
            e.HasOne(x => x.Movie).WithMany().HasForeignKey(x => x.MovieId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<Comment>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.UserEmail).HasMaxLength(200).IsRequired();
            e.Property(x => x.Text).HasMaxLength(1000).IsRequired();
            b.Entity<Comment>().HasOne(x => x.Movie).WithMany(m => m.Comments)
             .HasForeignKey(x => x.MovieId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}