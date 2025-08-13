using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class DeletePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
            .WithName("Posts : Delete")
            .WithSummary("Deletes a post")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(IPostHandler handler, int id)
    {
        var request = new DeletePostRequest
        {
            Id = id
        };
        var result = await handler.DeleteAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}