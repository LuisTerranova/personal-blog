using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.core;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class GetAllPostsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .WithName("Posts : GetAllPosts")
            .WithSummary("Gets all posts")
            .WithOrder(3);

    private static async Task<IResult> HandleAsync(IPostHandler handler, 
        int pageNumber = Configuration.DefaultPageNumber,
        int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllPostsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.NotFound(result);
    }
}