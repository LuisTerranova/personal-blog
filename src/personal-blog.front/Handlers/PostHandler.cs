using System.Net.Http.Json;
using System.Web;
using personal_blog.core.DTOs;
using personal_blog.core.Handlers;
using personal_blog.core.Handlers.FrontEndHandlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.front.Handlers;

public class PostHandler(IHttpClientFactory httpClientFactory) : IFrontPostHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public async Task<PagedResponse<List<PostDTO>?>> GetAllAsync(GetAllPostsRequest request)
    {
        var url = $"v1/posts?pageNumber={request.PageNumber}&pageSize={request.PageSize}";
        
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            url += $"&query={HttpUtility.UrlEncode(request.Query)}";
        }
        return await _client.GetFromJsonAsync<PagedResponse<List<PostDTO>?>>(url)
               ?? new PagedResponse<List<PostDTO>?>(null, "Could not fetch posts", 400);
    }

    public async Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Post?>>($"v1/posts/{request.Id}")
            ?? new Response<Post?>(null, "Could not get requested post", 400);

    public async Task<PagedResponse<List<PostDTO>?>> GetFeaturedAsync(GetFeaturedPostsRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<PostDTO>?>>("v1/posts/featured")
            ?? new PagedResponse<List<PostDTO>?>(null, "Could not fetch posts", 400);
}