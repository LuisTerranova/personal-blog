using System.ComponentModel.DataAnnotations;
using personal_blog.Api.Models;

namespace personal_blog.core.Models;

public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public List<Post> Posts { get; set; } 
    public long UserId { get; set; }
    public ApplicationUser User { get; set; }
}