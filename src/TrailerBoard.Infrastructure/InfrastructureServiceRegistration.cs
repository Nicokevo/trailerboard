using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailerBoard.Application;
using TrailerBoard.Infrastructure.Data;

namespace TrailerBoard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<TrailerDbContext>(o =>
            o.UseSqlite(cfg.GetConnectionString("db") ?? "Data Source=trailerboard.db"));

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<TrailerDbContext>());
        return services;
    }
}