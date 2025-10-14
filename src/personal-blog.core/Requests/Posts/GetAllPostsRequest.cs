namespace personal_blog.core.Requests.Posts;

public class GetAllPostsRequest : PagedRequest
{
    public string? Query { get; set; }
}
