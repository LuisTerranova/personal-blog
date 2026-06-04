namespace TrashTechHub.Core.DTOs;

public class PostDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; } = null!;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
}
