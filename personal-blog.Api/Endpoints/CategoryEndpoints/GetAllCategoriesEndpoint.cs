using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Categories : GetAll")
            .WithSummary("Get all categories")
            .WithOrder(3);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, GetAllCategoriesRequest request)
    {
        var result = await handler.GetAllAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.NotFound();
    }
}