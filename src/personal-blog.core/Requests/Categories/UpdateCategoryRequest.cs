using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Categories;

public class UpdateCategoryRequest : BaseRequest
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;
}