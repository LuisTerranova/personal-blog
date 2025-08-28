using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Categories;

public class CreateCategoryRequest : BaseRequest
{
    [Required]
    [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
    public required string Title { get; set; }
}