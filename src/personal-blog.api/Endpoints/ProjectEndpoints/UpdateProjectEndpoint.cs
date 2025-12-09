using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class UpdateProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .WithValidation<UpdateProjectRequest>()
            .WithName("Projects : Update")
            .WithSummary("Updates a project")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(IProjectHandler handler
        ,UpdateProjectRequest request
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user
        ,int id)
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