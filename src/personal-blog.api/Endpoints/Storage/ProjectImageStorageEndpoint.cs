using personal_blog.Api.Common.Api;
using personal_blog.Api.Common.Api.Helpers;
using personal_blog.core.Handlers;

namespace personal_blog.Api.Endpoints.ProjectEndpoints;

public class ProjectImageStorageEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/upload", HandleAsync)
            .RequireAuthorization("AdminPolicy")
            .DisableAntiforgery()
            .WithName("Storage : Upload Image")
            .WithSummary("Uploads a project image to local storage")
            .WithOrder(1);
    
    private static async Task<IResult> HandleAsync(IProjectHandler handler, IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var result = await handler.UploadImageAsync(stream, file.FileName);
        
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}