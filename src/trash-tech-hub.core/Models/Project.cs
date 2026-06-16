namespace TrashTechHub.Core.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty; 
    public string Summary { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string RepoLink { get; set; } = string.Empty;
    public bool IsFeatured { get; set; } = false;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public string Slug { get; set; } = string.Empty;
}