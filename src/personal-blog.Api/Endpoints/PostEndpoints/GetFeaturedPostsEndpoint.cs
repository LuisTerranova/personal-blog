using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class GetFeaturedPostsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapGet("/featured", HandleAsync)
            .WithName("Posts : GetFeaturedPosts")
            .WithSummary("Gets 3 Featured posts")
            .WithOrder(6);
    private static async Task<IResult> HandleAsync(IPostHandler handler)
    {
        var request = new GetFeaturedPostsRequest();
        var result = await handler.GetFeaturedAsync(request);
      
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.NotFound(result);
    }
}