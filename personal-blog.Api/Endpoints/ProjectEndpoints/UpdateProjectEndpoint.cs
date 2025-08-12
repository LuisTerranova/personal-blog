using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class UpdateProjectEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
            .WithName("Projects : Update")
            .WithSummary("Updates a project")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(IProjectHandler handler, UpdateProjectRequest request, int id)
    {
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}