using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Projects;

public class UpdateProjectRequest : BaseRequest
{
    public long Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; }
    [MaxLength(200, ErrorMessage = "Project description cannot exceed 200 characters")]
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    [Required(ErrorMessage = "Your project needs a Repo Link")]
    public string RepoLink { get; set; }
}