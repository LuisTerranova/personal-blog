using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithValidation<UpdateCategoryRequest>()
            .WithName("Categories : Update")
            .WithSummary("Updates a category")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler
        , UpdateCategoryRequest request
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user
        ,HttpRequest httpRequest
        , int id)
    {
        var applicationUser = await userManager.GetUserAsync(user);
        if (applicationUser == null)
            return Results.Unauthorized();

        request.UserId = applicationUser.Id;
        request.Id = id;
        
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest();
    }
}