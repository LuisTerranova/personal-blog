using System.ComponentModel.DataAnnotations;

namespace personal_blog.core.Requests.Categories;

public class GetCategoryByIdRequest : BaseRequest
{
    [Required]
    public int Id { get; set; }
}