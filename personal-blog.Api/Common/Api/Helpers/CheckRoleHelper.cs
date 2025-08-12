using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.core.Responses;

namespace personal_blog.Api.Common.Helpers;

public static class CheckRoleHelper
{
    public static async Task<Response<bool>> CheckAdminAsync(ClaimsPrincipal user,
        UserManager<IdentityUser> userManager)
    {
        if (user.Identity is { IsAuthenticated: false })
            return new Response<bool>(false, "Unauthorized: User is not authenticated", 401);
        var identityUser = await userManager.GetUserAsync(user);

        if (identityUser == null)
            return new Response<bool>(false, "Unauthorized: User identity not found", 401);
        var isAdmin = await userManager.IsInRoleAsync(identityUser, "Admin");

        if (!isAdmin)
            return new Response<bool>(false, "Forbidden: Only Admin users can create a category", 403);
        
        
        return new Response<bool>(true, "User is authorized", 200);
    }
}