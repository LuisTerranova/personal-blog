using Microsoft.AspNetCore.Identity;

namespace personal_blog.Api.Models;

public class ApplicationUser : IdentityUser<long>
{
    public List<IdentityRole<long>>? Roles { get; set; }
}