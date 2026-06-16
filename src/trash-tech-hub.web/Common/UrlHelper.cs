using System;

namespace TrashTechHub.Web;

public static class UrlHelper
{
    public static string EnsureAbsoluteUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return string.Empty;
        }

        url = url.Trim();

        if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
            url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        return "https://" + url;
    }
}
