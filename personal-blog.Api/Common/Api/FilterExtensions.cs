using personal_blog.Api.Common.Api.Filters;

namespace personal_blog.Api.Common.Api;

public static class FilterExtensions
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
    public static RouteHandlerBuilder RequireRole(this RouteHandlerBuilder builder, string requiredRole)
    {
        return builder.AddEndpointFilter(new RoleAuthorizationEndpointFilter(requiredRole));
    }
}