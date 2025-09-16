using Microsoft.AspNetCore.Identity;

namespace personal_blog.core.Models;

public class ApplicationUser : IdentityUser<long>
{
    public override long Id { get; set; }
    public List<IdentityRole<long>>? Roles { get; set; }
    public Dictionary<string, string> Claims { get; set; } = [];
}