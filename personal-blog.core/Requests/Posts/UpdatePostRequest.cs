using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Posts;

public class UpdatePostRequest : BaseRequest
{
    public long Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(200,  ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Article body is required")]
    public string Body { get; set; }
    [Required(ErrorMessage = "Select at least one category")]
    public List<int> CategoryIds { get; set; } = [];
}