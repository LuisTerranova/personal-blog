using System.ComponentModel.DataAnnotations;

namespace TrashTechHub.Core.Requests.Categories;

public class GetCategoryByIdRequest : BaseRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid category Id")]
    public int Id { get; set; }
}