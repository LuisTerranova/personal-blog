using Microsoft.AspNetCore.Identity;
using personal_blog.core.Models;

namespace personal_blog.Api.Models;

public class ApplicationUser : IdentityUser<long>
{
    public override long Id { get; set; }
    public List<IdentityRole<long>>? Roles { get; set; }
}