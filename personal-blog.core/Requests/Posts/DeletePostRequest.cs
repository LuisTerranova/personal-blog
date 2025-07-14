namespace personal_blog.core.Requests.Posts;

public class DeletePostRequest : BaseRequest
{
    public long Id { get; set; }
}