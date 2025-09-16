using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Helpers;
using personal_blog.core.Models;
using personal_blog.core.DTOs;
using personal_blog.core.Handlers;
using personal_blog.core.Requests.Categories;

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
        
        request.UserId = user!.Id;
        var result = await handler.CreateAsync(request);

        if (!result.IsSuccess) return TypedResults.BadRequest(result.Message);
        var responseDto = new ResponseDto
        {
            Id = result.Data!.Id,
            Title = result.Data.Title,
            Message = result.Message,
            StatusCode = result.StatusCode
        };   
        var location = LocationHelper.Location(httpRequest, "category", responseDto.Id);
        return TypedResults.Created(location, responseDto);
    }
}
