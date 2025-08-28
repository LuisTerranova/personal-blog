using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Models;

namespace personal_blog.Api.Common.Api.Filters;

public class RoleAuthorizationEndpointFilter(string requiredRole) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var user = context.HttpContext.User;
        
        if (user.Identity?.IsAuthenticated != true)
        {
            return Results.Unauthorized(); 
        }
        
        var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
        var applicationUser = await userManager.GetUserAsync(user);

        if (applicationUser == null)
        {
            return Results.Unauthorized(); 
        }

        var roles = await userManager.GetRolesAsync(applicationUser);

        if (!roles.Contains(requiredRole))
        {
            return Results.Forbid(); 
        }
        context.HttpContext.Items["ApplicationUser"] = applicationUser;
        
        return await next(context);
    }
}