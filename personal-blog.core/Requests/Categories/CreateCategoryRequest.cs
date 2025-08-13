namespace personal_blog.core.Requests.Categories;

public class CreateCategoryRequest : BaseRequest
{
    public required string Title { get; set; }
    public required string Slug { get; set; }
}