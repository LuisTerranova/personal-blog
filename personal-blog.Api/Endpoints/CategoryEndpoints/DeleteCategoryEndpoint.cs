using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.Api.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class DeleteCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapDelete("/{id}", HandleAsync)
            .AddEndpointFilter<AdminAuthorizationEndpointFilter>()
            .WithName("Categories : Delete")
            .WithSummary("Deletes a category")
            .WithOrder(2);

    private static async Task<IResult> HandleAsync(CategoryHandler handler, DeleteCategoryRequest request)
    {
        var result =  await handler.DeleteAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}