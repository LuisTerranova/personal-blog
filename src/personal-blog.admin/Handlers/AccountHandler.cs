using System.Net.Http.Json;
using System.Text;
using personal_blog.core.Handlers;
using personal_blog.core.Requests;
using personal_blog.core.Responses;

namespace personal_blog.admin.Handlers;

public class AccountHandler(IHttpClientFactory httpClientFactory) : IAccountHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    
    public async Task<Response<string>> LoginAsync(LoginRequest request)
    {
        var result = await _client.PostAsJsonAsync("v1/identity/login?useCookies=true", request);
        return result.IsSuccessStatusCode
            ? new Response<string>("Successful login", "Successful login")
            : new Response<string>("Unsuccessful login", "Unsuccessful login", 400);
    }

    public async Task LogoutAsync()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, "application/json");
        await _client.PostAsJsonAsync("v1/identity/logout", emptyContent);
    }
}