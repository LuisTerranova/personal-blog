using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Filters;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

namespace personal_blog.Api.Endpoints.CategoryEndpoints;

public class UpdateCategoryEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) 
        => app.MapPut("/{id}", HandleAsync)
            .AddEndpointFilter<RoleAuthorizationEndpointFilter>()
            .WithName("Categories : Update")
            .WithSummary("Updates a category")
            .WithOrder(5);

    private static async Task<IResult> HandleAsync(ICategoryHandler handler, UpdateCategoryRequest request, int id)
    {
        request.Id = id;
        var result = await handler.UpdateAsync(request);
        return result.IsSuccess 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest();
    }
}