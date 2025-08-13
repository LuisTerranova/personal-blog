using personal_blog.Api.Models;

namespace personal_blog.core.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string RepoLink { get; set; }
    public DateTime Created { get; set; }
    public long UserId { get; set; }
    public ApplicationUser User { get; set; }
}