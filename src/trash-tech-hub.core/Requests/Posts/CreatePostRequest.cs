using System.ComponentModel.DataAnnotations;

namespace TrashTechHub.Core.Requests.Posts;

public class CreatePostRequest : BaseRequest
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;
    [Required(ErrorMessage = "Article body is required")]
    public string Body { get; set; } = string.Empty;
    [Range(1, int.MaxValue, ErrorMessage = "Enter a valid category")]
    public int CategoryId { get; set; }
    public bool IsFeatured { get; set; }
    public string? Slug { get; set; }
    public string? Excerpt { get; set; }
}