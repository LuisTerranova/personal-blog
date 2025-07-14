namespace personal_blog.core.Requests.Categories;

public class UpdateCategoryRequest
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
}