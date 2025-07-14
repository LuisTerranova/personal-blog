namespace personal_blog.core.Models;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}