namespace personal_blog.core;

public class Configuration
{
    public const int DefaultStatusCode = 200;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 5;
    
    public static string ConnectionString { get; set; } = string.Empty;
}