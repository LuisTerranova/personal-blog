namespace personal_blog.core.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime Created { get; set; } =  DateTime.UtcNow;
    public DateTime Updated { get; set; } = DateTime.UtcNow;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public long UserId { get; set; }
}