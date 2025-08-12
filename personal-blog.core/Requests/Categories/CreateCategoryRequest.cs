namespace personal_blog.core.Requests.Categories;

public class CreateCategoryRequest : BaseRequest
{
    public string Title { get; set; }
    public string Slug { get; set; }
}