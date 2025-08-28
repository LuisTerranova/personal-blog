using System.Security.Claims;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class UpdatePostEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
            .WithName("Posts : Update")
            .WithSummary("Updates a post")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(IPostHandler handler, UpdatePostRequest request, int id,
        ClaimsPrincipal user)
    {
        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null || !long.TryParse(userIdString, out var userId)) return TypedResults.Unauthorized();
        
        request.UserId = userId;
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.BadRequest(result);
    }
}