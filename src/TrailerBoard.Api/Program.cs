using MediatR;
using Microsoft.EntityFrameworkCore;
using TrailerBoard.Infrastructure;
using TrailerBoard.Application;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Infra & Application
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
