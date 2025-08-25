using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailerBoard.Application;
using TrailerBoard.Application.Favorites;
using TrailerBoard.Infrastructure.Data;
using TrailerBoard.Infrastructure.Favorites;

namespace TrailerBoard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<TrailerDbContext>(o =>
            o.UseSqlite(cfg.GetConnectionString("db") ?? "Data Source=trailerboard.db"));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<TrailerDbContext>());
        services.AddScoped<IFavoritesService, FavoritesService>();

        return services;
    }
}