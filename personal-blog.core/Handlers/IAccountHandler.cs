using personal_blog.core.Requests;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface IAccountHandler
{
    Task<Response<string>> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}