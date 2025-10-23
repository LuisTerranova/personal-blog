using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Helpers;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;
using ApplicationUser = personal_blog.Api.Models.ApplicationUser;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class CreateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPost("/", HandleAsync)
            .RequireRole("admin")
            .WithValidation<CreateCategoryRequest>()
            .WithName("Categories : Create")
            .WithSummary("Creates a new category")
            .WithOrder(1);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler
        ,CreateCategoryRequest request
        ,HttpContext httpContext
        ,HttpRequest httpRequest)
    {
        var user = httpContext.Items["ApplicationUser"] as ApplicationUser;
        if (user == null) return TypedResults.Unauthorized();
        
        request.UserId = user.Id;
        
        var result = await handler.CreateAsync(request);

        if (!result.IsSuccess || result.Data?.Id == null) return TypedResults.BadRequest(result.Message);
        
        var location = LocationHelper.Location(httpRequest, "category", result.Data.Id);
        return TypedResults.Created(location, result);

    }
}
