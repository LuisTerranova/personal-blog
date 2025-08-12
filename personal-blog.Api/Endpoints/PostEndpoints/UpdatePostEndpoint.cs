using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class UpdatePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPost("/", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Posts : Update")
            .WithSummary("Updates a post")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(IPostHandler handler, UpdatePostRequest request)
    {
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}