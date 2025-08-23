using TrailerBoard.Infrastructure;
using TrailerBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using TrailerBoard.Application;  
using TrailerBoard.Contracts;   



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// EF Core (Infrastructure)
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

var api = app.MapGroup("/api");

api.MapGet("/movies", (IAppDbContext db, string? query) =>
{
    var q = db.Movies;
    if (!string.IsNullOrWhiteSpace(query))
        q = q.Where(m => EF.Functions.Like(m.Title, $"%{query}%"));

    var list = q.OrderBy(m => m.Title)
        .Select(m => new MovieDto(m.PublicId, m.Title, m.Year, m.PosterUrl, m.TrailerUrl))
        .ToList();

    return Results.Ok(list);
})
.WithName("GetMovies");


// migrate + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TrailerDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.Run();
