namespace personal_blog.core.Requests.Categories;

public class UpdateCategoryRequest : BaseRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
}