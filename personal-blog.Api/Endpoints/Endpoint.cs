using personal_blog.Api.Common.Api;
using personal_blog.Api.Endpoints.AccountEndpoints;
using personal_blog.Api.Endpoints.CategoryEndpoints;
using personal_blog.Api.Endpoints.PostEndpoints;
using personal_blog.Api.Endpoints.ProjectEndpoints;
using personal_blog.core.Models;

namespace personal_blog.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.MapGroup("");

        endpoints.MapGroup("v1/categories")
            .WithTags("Categories")
            .MapEndpoint<CreateCategoryEndpoint>()
            .MapEndpoint<DeleteCategoryEndpoint>()
            .MapEndpoint<GetAllCategoriesEndpoint>()
            .MapEndpoint<GetCategoryByIdEndpoint>()
            .MapEndpoint<UpdateCategoryEndpoint>();
        endpoints.MapGroup("v1/posts")
            .WithTags("Posts")
            .MapEndpoint<CreatePostEndpoint>()
            .MapEndpoint<DeletePostEndpoint>()
            .MapEndpoint<GetAllPostsEndpoint>()
            .MapEndpoint<GetFeaturedPostsEndpoint>()
            .MapEndpoint<GetPostByIdEndpoint>()
            .MapEndpoint<UpdatePostEndpoint>();
        endpoints.MapGroup("v1/projects")
            .WithTags("Projects")
            .MapEndpoint<CreateProjectEndpoint>()
            .MapEndpoint<DeleteProjectEndpoint>()
            .MapEndpoint<GetAllProjectsEndpoint>()
            .MapEndpoint<UpdateProjectEndpoint>();
        endpoints.MapGroup("v1/identity").MapIdentityApi<ApplicationUser>();
        endpoints.MapGroup("v1/logout").MapEndpoint<LogoutEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);
        return app;
    }
}