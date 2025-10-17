using personal_blog.core.Models;

namespace personal_blog.core.DTOs;

public class PostDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public DateTime Created { get; set; } =  DateTime.Now;
    public DateTime Updated { get; set; } = DateTime.Now;
    public Category? Category { get; set; }
}