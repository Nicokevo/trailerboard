// ── Usings (infraestructura, EF, DTOs y JWT) ───────────────────────────────────
using Microsoft.EntityFrameworkCore;
using TrailerBoard.Infrastructure;         // AddInfrastructure()
using TrailerBoard.Infrastructure.Data;    // TrailerDbContext, DbSeeder
using TrailerBoard.Application;            // IAppDbContext
using TrailerBoard.Contracts;              // DTOs

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// ── Builder + Servicios (OpenAPI, Infra/EF, Auth) ──────────────────────────────
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();             // OpenAPI del template

// EF Core (registrado desde la capa Infrastructure)
builder.Services.AddInfrastructure(builder.Configuration);

// JWT (dev): leemos config y registramos autenticación
var jwt = builder.Configuration.GetSection("Jwt");
var signingKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("Missing Jwt:Key")));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromSeconds(5)
        };
    });

builder.Services.AddAuthorization();

// ── Build + bootstrap de base de datos (migrar + seed) ─────────────────────────
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TrailerDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db); // inserta desde JSON embebido si está vacío
}

// OpenAPI solo en Development (y evitamos el warning de HTTPS en dev)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

// Middlewares de auth
app.UseAuthentication();
app.UseAuthorization();

// ── Endpoints ──────────────────────────────────────────────────────────────────
var api = app.MapGroup("/api");

// 1) Público: listado de películas con filtro opcional (?query=)
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

// 2) Auth: login que emite un JWT HS256 (dev)
api.MapPost("/auth/login", (LoginRequest req) =>
{
    if (string.IsNullOrWhiteSpace(req.Email) ||
        string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 6)
        return Results.BadRequest(new { message = "Invalid credentials" });

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, req.Email),
        new Claim(JwtRegisteredClaimNames.Email, req.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: jwt["Issuer"],
        audience: jwt["Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddHours(4),
        signingCredentials: creds
    );

    var jwtString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new LoginResponse(jwtString, req.Email));
})
.WithName("Login");

// 3) Protegido de ejemplo: /api/me (requiere Authorization: Bearer <token>)
api.MapGet("/me", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email)
        ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);
    return Results.Ok(new { email });
})
.RequireAuthorization()
.WithName("Me");

app.Run();
