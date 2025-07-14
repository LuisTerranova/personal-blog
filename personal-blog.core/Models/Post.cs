namespace personal_blog.core.Models;

public class Post
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime Created { get; set; } =  DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public long UserId { get; set; }
}