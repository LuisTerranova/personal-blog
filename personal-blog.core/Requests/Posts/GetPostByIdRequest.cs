namespace personal_blog.core.Requests.Posts;

public class GetPostByIdRequest : BaseRequest
{
    public long Id { get; set; }
}