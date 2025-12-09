using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Models;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class UpdatePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id:int}", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithValidation<UpdatePostRequest>()
            .WithName("Posts : Update")
            .WithSummary("Updates a post")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(IPostHandler handler
        ,UpdatePostRequest request
        ,int id
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user)
    {
        var applicationUser = userManager.GetUserAsync(user);
        request.UserId = applicationUser.Id;
        request.Id = id;
        
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}