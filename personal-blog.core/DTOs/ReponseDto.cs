namespace personal_blog.core.DTOs;

public class ResponseDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}