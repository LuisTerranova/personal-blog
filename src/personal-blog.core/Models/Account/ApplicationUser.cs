namespace personal_blog.core.Models;

public class ApplicationUser
{
    public string Email { get; set; } = string.Empty;
    public Dictionary<string, string> Claims { get; set; } = [];
}