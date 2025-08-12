using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class GetAllPostsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Posts : GetAllPosts")
            .WithSummary("Gets all posts")
            .WithOrder(3);

    private static async Task<IResult> HandleAsync(IPostHandler handler, GetAllPostsRequest request)
    {
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.NotFound();
    }
}