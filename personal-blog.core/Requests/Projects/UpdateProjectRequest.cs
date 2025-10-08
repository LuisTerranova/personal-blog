using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;

namespace personal_blog.core.Requests.Projects;

public class UpdateProjectRequest : BaseRequest
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; }
    [MaxLength(200, ErrorMessage = "Project description cannot exceed 200 characters")]
    public string Description { get; set; } = string.Empty;
    [JsonIgnore]
    public IBrowserFile? ImageFile { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    [Required(ErrorMessage = "Your project needs a Repo Link")]
    public string RepoLink { get; set; }
}