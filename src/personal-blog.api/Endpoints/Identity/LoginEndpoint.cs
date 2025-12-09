using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Models;

namespace personal_blog.Api.Endpoints.Identity;

public record LoginRequest(string Email, string Password);

public class LoginEndpoint : IEndpoint 
{
    public static void Map(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", HandleAsync)
            .AllowAnonymous() 
            .WithTags("Identity");
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] LoginRequest request,
        SignInManager<ApplicationUser> signInManager)
    {
        var result = await signInManager.PasswordSignInAsync(
            request.Email, 
            request.Password, 
            isPersistent: true,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
            return Results.Ok(new { message = "Login Successful" });

        return result.IsLockedOut 
            ? Results.Problem("Too many failed attempts, retry later", statusCode: 429) 
            : Results.Unauthorized();
    }
}