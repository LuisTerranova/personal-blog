namespace personal_blog.core.Requests;

public class PagedRequest : BaseRequest
{
    public int PageNumber { get; set; } = Configuration.DefaultPageNumber;
    public int PageSize { get; set; } = Configuration.DefaultPageSize;
}