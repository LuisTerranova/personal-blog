using System.Net.Http.Json;
using System.Web;
using personal_blog.core.Handlers;
using personal_blog.core.Handlers.FrontEndHandlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.front.Handlers;

public class CategoryHandler(IHttpClientFactory httpClientFactory) : IFrontCategoryHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var url = $"v1/categories?pageNumber={request.PageNumber}&pageSize={request.PageSize}";
        
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            url += $"&query={HttpUtility.UrlEncode(request.Query)}";
        }
        
        return await _client.GetFromJsonAsync<PagedResponse<List<Category>?>>(url)
               ?? new PagedResponse<List<Category>?>(null, "Could not fetch posts", 400);
    }
    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Category?>>($"v1/categories/{request.Id}")
           ?? new Response<Category?>(null, "Could not get requested category", 400);
}