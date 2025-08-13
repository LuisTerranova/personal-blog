using System.Security.Claims;
using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class CreatePostEndpoint : IEndpoint
{ 
    public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/", HandleAsync)
                .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
                .WithName("Posts : Create")
                .WithSummary("Creates a new post")
                .WithOrder(1);

    private static async Task<IResult> HandleAsync(IPostHandler handler, CreatePostRequest request,
        ClaimsPrincipal user)
    {
        var userIdString = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdString == null || !long.TryParse(userIdString, out var userId)) return TypedResults.Unauthorized();
        
        request.UserId = userId;
        var result = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data.Id}", result) 
            : TypedResults.BadRequest(result);
    }
}