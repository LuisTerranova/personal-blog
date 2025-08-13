using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Projects;

public class CreateProjectRequest : BaseRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public required string Title { get; set; }
    [MaxLength(200, ErrorMessage = "Project description cannot exceed 200 characters")]
    public required string Description { get; set; } = string.Empty;
    public required string ImageUrl { get; set; } = string.Empty;
    [Required(ErrorMessage = "Your project needs a Repo Link")]
    public required string RepoLink { get; set; }
}