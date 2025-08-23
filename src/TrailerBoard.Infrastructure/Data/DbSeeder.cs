using System.Reflection;
using System.Text.Json;
using TrailerBoard.Domain;

namespace TrailerBoard.Infrastructure.Data;

public static class DbSeeder
{
    public static void Seed(TrailerDbContext db)
    {
        if (db.Movies.Any()) return;

        var asm = Assembly.GetExecutingAssembly();
        var resource = asm.GetManifestResourceNames()
            .First(n => n.EndsWith("Seed.movies.json", StringComparison.OrdinalIgnoreCase));

        using var s = asm.GetManifestResourceStream(resource)!;
        using var r = new StreamReader(s);
        var json = r.ReadToEnd();

        var items = JsonSerializer.Deserialize<List<Movie>>(json) ?? new();
        foreach (var m in items) { m.Id = 0; m.Comments = new List<Comment>(); }

        db.Movies.AddRange(items);
        db.SaveChanges();
    }
}