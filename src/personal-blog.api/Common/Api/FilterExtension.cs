using personal_blog.Api.Common.Api.Filters;

namespace personal_blog.Api.Common.Api;

public static class FilterExtension
{
    public static RouteHandlerBuilder WithValidation<T>(this RouteHandlerBuilder builder) where T : class
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}