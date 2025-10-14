using Microsoft.AspNetCore.Components.Forms;

namespace personal_blog.core.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string RepoLink { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public long UserId { get; set; }
}