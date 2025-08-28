using personal_blog.core.DTOs;

namespace personal_blog.Api.Common.Api.Helpers;

public static class LocationHelper
{
    public static string Location(HttpRequest request, string entity, long entityId)
    {
        var builder = new UriBuilder(request.Scheme, request.Host.Host, request.Host.Port ?? -1);
        
        if (!string.IsNullOrEmpty(request.PathBase))
            builder.Path = request.PathBase;
        builder.Path = $"{builder.Path.TrimEnd('/')}/{entity}/{entityId}";
        var location = builder.Uri.ToString();

        return location;
    }
}