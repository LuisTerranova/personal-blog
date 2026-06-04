using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrashTechHub.Core.Services;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Infrastructure.Services;

public class AuthService(
    AppDbContext context,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<bool> ValidateAdminAsync(string email, string password)
    {
        var admin = await context.AdminUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);

        if (admin == null) return false;

        var valid = BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
        if (!valid) logger.LogWarning("Failed login attempt for {Email}", email);
        return valid;
    }
}
