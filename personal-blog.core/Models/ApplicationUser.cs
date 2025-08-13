using Microsoft.AspNetCore.Identity;
using personal_blog.core.Models;

namespace personal_blog.Api.Models;

public class ApplicationUser : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; }
    public List<Post>? Posts { get; set; }
    public List<Project>?  Projects { get; set; }
}