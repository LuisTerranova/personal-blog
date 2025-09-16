using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.core.Models;

namespace personal_blog.Api.Endpoints.AccountEndpoints;

public class LogoutEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .RequireAuthorization();

    private static async Task<IResult> HandleAsync(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }
}