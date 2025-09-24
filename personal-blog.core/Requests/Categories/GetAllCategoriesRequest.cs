namespace personal_blog.core.Requests.Categories;

public class GetAllCategoriesRequest : PagedRequest
{
    public string? Query { get; set; }
};
