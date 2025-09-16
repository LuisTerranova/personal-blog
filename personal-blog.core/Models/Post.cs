namespace personal_blog.core.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Created { get; set; } =  DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    
    public Category Category { get; set; }
    public int CategoryId { get; set; }
    public long UserId { get; set; }
    public ApplicationUser User { get; set; }
}