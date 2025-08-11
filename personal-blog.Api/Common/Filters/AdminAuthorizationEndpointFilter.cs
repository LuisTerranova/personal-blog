using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Helpers;

namespace personal_blog.Api.Common.Filters;

public class AdminAuthorizationEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
        var user = context.HttpContext.User;

        var authResponse = await CheckRoleHelper.CheckAdminAsync(user, userManager);

        if (!authResponse.IsSuccess)
            return Results.Problem(detail: authResponse.Message, statusCode: authResponse.StatusCode);
        
        return await next(context);
    }
}