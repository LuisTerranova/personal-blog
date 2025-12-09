using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class DeleteProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithName("Projects : Delete")
            .WithSummary("Deletes a project")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(IProjectHandler handler
        ,int id
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user)
    {
        var applicationUser = userManager.GetUserAsync(user);
        
        var request = new DeleteProjectRequest
        {
            UserId = applicationUser.Id,
            Id = id
        };
        var result = await handler.DeleteAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}