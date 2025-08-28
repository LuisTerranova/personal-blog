using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapGet("/{id}", HandleAsync)
            .WithName("Categories : GetById")
            .WithSummary("Get a category by its id")
            .WithOrder(4);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, int id)
    {
        var request = new GetCategoryByIdRequest
        {
            Id = id
        };
        var result = await handler.GetByIdAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok(result) 
            : TypedResults.NotFound(result);
    }
}