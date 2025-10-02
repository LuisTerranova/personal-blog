using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class GetPostByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/{id:int}", HandleAsync)
            .WithName("Posts : GetPostById")
            .WithSummary("Get a post by id")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(IPostHandler handler, int id)
    {
        var request = new GetPostByIdRequest
        {
            Id = id
        };
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.NotFound(result);
    }
}