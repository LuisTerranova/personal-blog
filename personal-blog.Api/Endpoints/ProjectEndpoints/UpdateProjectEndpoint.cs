using personal_blog.Api.Common.Api;
using personal_blog.Api.Models;
using personal_blog.core.Models;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class UpdateProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .RequireRole("admin")
            .WithValidation<UpdateProjectRequest>()
            .WithName("Projects : Update")
            .WithSummary("Updates a project")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(IProjectHandler handler
        ,UpdateProjectRequest request
        ,HttpContext httpContext
        ,int id)
    {
        var user = httpContext.Items["ApplicationUser"] as ApplicationUser;
        
        request.UserId = user!.Id;
        request.Id = id;
        
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}