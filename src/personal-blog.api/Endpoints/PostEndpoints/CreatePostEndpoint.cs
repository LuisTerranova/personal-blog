using personal_blog.Api.Common.Api;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Posts;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.PostEndpoints;

public class CreatePostEndpoint : IEndpoint
{ 
    public static void Map(IEndpointRouteBuilder app) 
            => app.MapPost("/", HandleAsync)
                .RequireRole("admin")
                .WithValidation<CreatePostRequest>()
                .WithName("Posts : Create")
                .WithSummary("Creates a new post")
                .WithOrder(1);

    private static async Task<IResult> HandleAsync(IPostHandler handler
        ,CreatePostRequest request
        ,HttpContext httpContext)
    {
        var user = httpContext.Items["ApplicationUser"] as ApplicationUser;
        if (user == null) return TypedResults.Unauthorized();
        
        request.UserId = user.Id;
        
        var result = await handler.CreateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Created($"/{result.Data.Id}", result) 
            : TypedResults.BadRequest(result);
    }
}