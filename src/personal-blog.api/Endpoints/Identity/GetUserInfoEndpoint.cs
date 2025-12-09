using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Models;

namespace personal_blog.Api.Endpoints.Identity;

public class GetUserInfoEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/manage/info", HandleAsync)
            .RequireAuthorization() 
            .WithTags("Identity");
    }

    private static async Task<IResult> HandleAsync(
        UserManager<ApplicationUser> userManager,
        ClaimsPrincipal claimsUser)
    {
        var user = await userManager.GetUserAsync(claimsUser);
        
        if (user is null) 
            return Results.Unauthorized();
        
        var roles = await userManager.GetRolesAsync(user);
        var claims = await userManager.GetClaimsAsync(user);
        
        var claimsDictionary = new Dictionary<string, string>
        {
            { ClaimTypes.Name, user.Email! },
            { ClaimTypes.Email, user.Email! }
        };

        foreach (var role in roles)
        {
            claimsDictionary.Add(ClaimTypes.Role, role);
        }
        
        foreach (var claim in claims)
        {
            if (!claimsDictionary.ContainsKey(claim.Type))
            {
                claimsDictionary.Add(claim.Type, claim.Value);
            }
        }
        
        return Results.Ok(new 
        {
            user.Email,
            Claims = claimsDictionary
        });
    }
}