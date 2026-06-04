namespace TrashTechHub.Core.Requests.Categories;

public class GetAllCategoriesRequest : PagedRequest
{
    public string? Query { get; set; }
};
