using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Models;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class CreateProjectEndpoint : IEndpoint
{ 
    public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/", HandleAsync)
                .RequireAuthorization("AdminPolicy")
                .WithValidation<CreateProjectRequest>()
                .WithName("Projects : Create")
                .WithSummary("Creates a new project")
                .WithOrder(1);

    private static async Task<IResult> HandleAsync(IProjectHandler handler
        ,CreateProjectRequest request
        ,UserManager<ApplicationUser> userManager
        ,ClaimsPrincipal user)
    {
        var applicationUser = userManager.GetUserAsync(user);
        if (applicationUser == null) return TypedResults.Unauthorized();
        
        request.UserId = applicationUser.Id;
        
        var result = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data.Id}", result) 
            : TypedResults.BadRequest(result);
    }
}