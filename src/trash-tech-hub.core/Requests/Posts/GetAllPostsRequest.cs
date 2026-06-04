namespace TrashTechHub.Core.Requests.Posts;

public class GetAllPostsRequest : PagedRequest
{
    public string? Query { get; set; }
    public int? CategoryId { get; set; }
}
