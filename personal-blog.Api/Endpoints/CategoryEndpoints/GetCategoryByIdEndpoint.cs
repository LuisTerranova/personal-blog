using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Categories : GetById")
            .WithSummary("Get a category by its id")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, GetCategoryByIdRequest request)
    {
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.NotFound();
    }
}