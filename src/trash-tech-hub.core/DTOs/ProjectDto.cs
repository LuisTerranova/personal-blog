namespace TrashTechHub.Core.DTOs;

public class ProjectDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string RepoLink { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public string Slug { get; set; } = string.Empty;
}
