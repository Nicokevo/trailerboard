using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc;

namespace TrailerBoard.Api.Validation;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext ctx, EndpointFilterDelegate next)
    {
        var validator = ctx.HttpContext.RequestServices.GetService(typeof(IValidator<T>)) as IValidator<T>;
        if (validator is null) return await next(ctx);

        // Buscar el argumento del tipo T en los parámetros del handler
        foreach (var arg in ctx.Arguments)
        {
            if (arg is T model)
            {
                var result = await validator.ValidateAsync(model);
                if (!result.IsValid)
                {
                    var problem = new ValidationProblemDetails();
                    foreach (var err in result.Errors)
                        problem.Errors.Add(err.PropertyName, new[] { err.ErrorMessage });

                    problem.Status = StatusCodes.Status400BadRequest;
                    problem.Title = "validation_failed";
                    return Results.Problem(problem);
                }
                break;
            }
        }

        return await next(ctx);
    }
}
