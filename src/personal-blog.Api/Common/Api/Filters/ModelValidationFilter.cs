using MiniValidation;

namespace personal_blog.Api.Common.Api.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.GetArgument<T>(1);
        
        if (argument is null)
        {
            return Results.Problem("Request body cannot be empty.", statusCode: 400);
        }
        
        if (!MiniValidator.TryValidate(argument, out var errors))
        {
            return Results.ValidationProblem(errors);
        }
        
        return await next(context);
    }
}