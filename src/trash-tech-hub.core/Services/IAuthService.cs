namespace TrashTechHub.Core.Services;

public interface IAuthService
{
    Task<bool> ValidateAdminAsync(string email, string password);
}
