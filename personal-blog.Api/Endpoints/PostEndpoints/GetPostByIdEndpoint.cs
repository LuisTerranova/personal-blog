using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class GetPostByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Posts : GetPostById")
            .WithSummary("Get a post by id")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(IPostHandler handler, GetPostByIdRequest request)
    {
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.NotFound();
    }
}