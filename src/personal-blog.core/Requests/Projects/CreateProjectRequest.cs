using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using personal_blog.core.Common.Helpers;

namespace personal_blog.core.Requests.Projects;

public class CreateProjectRequest : IProjectRequestWithImage
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    [MaxLength(2000, ErrorMessage = "Project description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    [MaxLength(200, ErrorMessage = "Project summary cannot exceed 200 characters")]
    public string Summary { get; set; } = string.Empty;
    [Required(ErrorMessage = "Your project needs a Repo Link")]
    public string RepoLink { get; set; }
}