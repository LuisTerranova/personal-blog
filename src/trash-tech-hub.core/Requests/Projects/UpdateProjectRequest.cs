using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using TrashTechHub.Core.Common.Helpers;

namespace TrashTechHub.Core.Requests.Projects;

public class UpdateProjectRequest : ProjectRequestWithImageBase
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid project Id")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    [MaxLength(600, ErrorMessage = "Project summary cannot exceed 600 characters")]
    public string Summary { get; set; } = string.Empty;
    [MaxLength(2000, ErrorMessage = "Project description cannot exceed 2000 characters")]
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Your project needs a Repo Link")]
    public string RepoLink { get; set; } = string.Empty;
    public string? Slug { get; set; }
}