using System.ComponentModel.DataAnnotations;

namespace TrashTechHub.Core.Requests.Categories;

public class UpdateCategoryRequest : BaseRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid category Id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Category name cannot be longer than 100 characters.")]
    public string Title { get; set; } = string.Empty;
}