using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrailerBoard.Application.Abstractions.Persistence;
using TrailerBoard.Application.Abstractions.Security;
using TrailerBoard.Infrastructure.Persistence;
using TrailerBoard.Infrastructure.Persistence.Repositories;
using TrailerBoard.Infrastructure.Security;

namespace TrailerBoard.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {
        services.AddDbContext<TrailerDbContext>(o =>
            o.UseSqlite(cfg.GetConnectionString("db") ?? "Data Source=trailer board.db"));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();

        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        // JWT Token Generator
        services.AddScoped<ITokenGenerator, TokenGenerator>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
