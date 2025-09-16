using personal_blog.Api.Common.Api;
using personal_blog.core.Models;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class DeletePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .RequireRole("admin")
            .WithName("Posts : Delete")
            .WithSummary("Deletes a post")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(IPostHandler handler
        ,int id
        ,HttpContext httpContext)
    {
        var user = httpContext.Items["ApplicationUser"] as ApplicationUser;
        
        var request = new DeletePostRequest
        {
            UserId = user!.Id,
            Id = id
        };
        var result = await handler.DeleteAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}