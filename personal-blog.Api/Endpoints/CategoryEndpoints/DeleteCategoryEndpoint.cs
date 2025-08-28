using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.Api.Models;
using personal_blog.core.DTOs;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .RequireRole("admin")
            .WithName("Categories : Delete")
            .WithSummary("Deletes a category")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler
        , int id
        , ClaimsPrincipal principal
        , UserManager<ApplicationUser> userManager)
    {
        var user =  await userManager.GetUserAsync(principal);
        
        var request = new DeleteCategoryRequest
        {
            UserId = user.Id, 
            Id = id
        };
        var result =  await handler.DeleteAsync(request);
        var responseDto = new ResponseDto
        {
            Message = result.Message,
            StatusCode = result.StatusCode
        };
        
        return result.IsSuccess 
            ? TypedResults.Ok(responseDto) 
            : TypedResults.BadRequest(responseDto);
    }
}