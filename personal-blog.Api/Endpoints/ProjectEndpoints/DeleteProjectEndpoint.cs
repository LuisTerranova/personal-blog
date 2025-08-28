using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.Api.Models;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class DeleteProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .RequireRole("admin")
            .WithName("Projects : Delete")
            .WithSummary("Deletes a project")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(IProjectHandler handler
        ,int id
        ,HttpContext httpContext)
    {
        var user = httpContext.Items["ApplicationUser"] as ApplicationUser;
        
        var request = new DeleteProjectRequest
        {
            UserId = user!.Id,
            Id = id
        };
        var result = await handler.DeleteAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}