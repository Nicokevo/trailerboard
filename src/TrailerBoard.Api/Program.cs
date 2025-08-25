using Microsoft.EntityFrameworkCore;
using TrailerBoard.Infrastructure;         
using TrailerBoard.Infrastructure.Data;    
using TrailerBoard.Application;           
using TrailerBoard.Contracts;             
using TrailerBoard.Api.Errors;
using TrailerBoard.Api.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using TrailerBoard.Application.Favorites;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();            


builder.Services.AddInfrastructure(builder.Configuration);

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
builder.Services.AddProblemDetailsWithMappings();
builder.Services.AddValidatorsFromAssemblyContaining<TrailerBoard.Application.Validation.LoginRequestValidator>();


var app = builder.Build();

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
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseGlobalExceptionHandler();

static string? GetEmail(ClaimsPrincipal user) =>
    user.FindFirstValue(ClaimTypes.Email) ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);
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


api.MapGet("/me", (ClaimsPrincipal user) =>
{
    var email = user.FindFirstValue(ClaimTypes.Email)
        ?? user.FindFirstValue(JwtRegisteredClaimNames.Email);
    return Results.Ok(new { email });
})
.RequireAuthorization()
.WithName("Me");

api.MapPost("/favorites", async (AddFavoriteRequest req, IFavoritesService svc, ClaimsPrincipal user, CancellationToken ct) =>
{
    var email = GetEmail(user);
    if (string.IsNullOrWhiteSpace(email)) return Results.Unauthorized();

    var dto = await svc.AddAsync(email, req.PublicId, ct);   // 201 si ok
    return Results.Created($"/api/favorites/{dto.PublicId}", dto);
})
.AddEndpointFilter(new ValidationFilter<AddFavoriteRequest>())
.RequireAuthorization()
.WithName("AddFavorite");


api.MapGet("/favorites", async (IFavoritesService svc, ClaimsPrincipal user, CancellationToken ct) =>
{
    var email = GetEmail(user);
    if (string.IsNullOrWhiteSpace(email)) return Results.Unauthorized();

    var list = await svc.ListAsync(email, ct);
    return Results.Ok(list);
})
.RequireAuthorization()
.WithName("ListFavorites");

app.Run();

