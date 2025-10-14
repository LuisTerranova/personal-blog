using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class GetProjectByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/{id:int}", HandleAsync)
            .WithName("Projects : GetProjectById")
            .WithSummary("Get a project by id")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(IProjectHandler handler, int id)
    {
        var request = new GetProjectByIdRequest()
        {
            Id = id
        };
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.NotFound(result);
    }
}