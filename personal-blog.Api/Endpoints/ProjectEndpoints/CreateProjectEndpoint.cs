using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Projects;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class CreateProjectEndpoint : IEndpoint
{ 
    public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/", HandleAsync)
                .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
                .WithName("Projects : Create")
                .WithSummary("Creates a new project")
                .WithOrder(1);

    private static async Task<IResult> HandleAsync(IProjectHandler handler, CreateProjectRequest request)
    {
        var result = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created() 
            : TypedResults.BadRequest();
    }
}