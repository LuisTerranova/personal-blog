using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Helpers;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPost("/", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithValidation<CreateCategoryRequest>()
            .WithName("Categories : Create")
            .WithSummary("Creates a new category")
            .WithOrder(1);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler
        ,CreateCategoryRequest request
        ,HttpRequest httpRequest
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user)
    {
        var applicationUser = await userManager.GetUserAsync(user);
        if (applicationUser == null) return TypedResults.Unauthorized();
        
        request.UserId = applicationUser.Id;
        
        var result = await handler.CreateAsync(request);

        if (!result.IsSuccess || result.Data?.Id == null) return TypedResults.BadRequest(result.Message);
        
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data.Id}", result) 
            : TypedResults.BadRequest(result);

    }
}
