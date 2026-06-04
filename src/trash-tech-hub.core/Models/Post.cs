namespace TrashTechHub.Core.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime Created { get; set; } =  DateTime.UtcNow;
    public DateTime? Updated { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsFeatured { get; set; } = false;
    public string Slug { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
}