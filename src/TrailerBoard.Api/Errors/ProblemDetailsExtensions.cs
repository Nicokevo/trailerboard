using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

namespace TrailerBoard.Api.Errors;

public static class ProblemDetailsExtensions
{
    public static IServiceCollection AddProblemDetailsWithMappings(this IServiceCollection services)
    {
        services.AddProblemDetails(opts =>
        {
            opts.CustomizeProblemDetails = ctx =>
            {
                // Usa title = mensaje si llega
                if (ctx.Exception is not null && !string.IsNullOrWhiteSpace(ctx.Exception.Message))
                    ctx.ProblemDetails.Title = ctx.Exception.Message;
            };
        });
        return services;
    }

    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory? _ = null)
    {
        app.UseExceptionHandler(appErr =>
        {
            appErr.Run(async httpContext =>
            {
                var ex = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
                var (status, title) = ex switch
                {
                    KeyNotFoundException   => (StatusCodes.Status404NotFound, ex!.Message is { Length: > 0 } ? ex.Message : "not_found"),
                    InvalidOperationException => (StatusCodes.Status409Conflict, ex!.Message is { Length: > 0 } ? ex.Message : "conflict"),
                    _ => (StatusCodes.Status500InternalServerError, "unexpected_error")
                };

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Type = "about:blank",
                    Detail = status == 500 ? "An unexpected error occurred." : null
                };

                httpContext.Response.StatusCode = status;
                httpContext.Response.ContentType = "application/problem+json";
                await httpContext.Response.WriteAsJsonAsync(problem);
            });
        });
        return app;
    }
}
