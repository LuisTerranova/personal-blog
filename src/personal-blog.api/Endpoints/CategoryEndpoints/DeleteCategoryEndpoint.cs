using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithName("Categories : Delete")
            .WithSummary("Deletes a category")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler
        ,int id
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user)
    {
        var applicationUser = await userManager.GetUserAsync(user);
        if (applicationUser == null) return TypedResults.Unauthorized();
        
        var request = new DeleteCategoryRequest
        {
            UserId = applicationUser.Id, 
            Id = id
        };
        var result =  await handler.DeleteAsync(request);
        
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);

    }
}