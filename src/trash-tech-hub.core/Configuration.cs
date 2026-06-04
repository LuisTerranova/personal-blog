namespace TrashTechHub.Core;

public class Configuration
{
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 5;
    
    public static string ConnectionString { get; set; } = "Host=localhost;Port=5432;Database=trash_tech_hub;Username=postgres;Password=postgres";
}