namespace personal_blog.core.Requests.Posts;

public class GetPostByIdRequest : BaseRequest
{
    public int Id { get; set; }
}