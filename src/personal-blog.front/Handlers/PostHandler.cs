using System.Net.Http.Json;
using System.Web;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.front.Handlers;

public class PostHandler(IHttpClientFactory httpClientFactory) : IPostHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public async Task<Response<Post?>> CreateAsync(CreatePostRequest request)
    {  
        var result =  await _client.PostAsJsonAsync("v1/posts", request);
        return await result.Content.ReadFromJsonAsync<Response<Post>?>()
            ?? new Response<Post?>(null, "Could not create post", 400);
    }

    public async Task<Response<Post?>> DeleteAsync(DeletePostRequest request)
    {
        var result = await _client.DeleteAsync($"v1/posts/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Post>?>()
            ??  new Response<Post?>(null, "Could not delete requested post", 400);
    }

    public async Task<PagedResponse<List<Post>?>> GetAllAsync(GetAllPostsRequest request)
    {
        var url = $"v1/posts?pageNumber={request.PageNumber}&pageSize={request.PageSize}";
        
        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            url += $"&query={HttpUtility.UrlEncode(request.Query)}";
        }
        return await _client.GetFromJsonAsync<PagedResponse<List<Post>>?>(url)
               ?? new PagedResponse<List<Post>>(null, "Could not fetch posts", 400);
    }

    public async Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Post?>>($"v1/posts/{request.Id}")
            ?? new Response<Post?>(null, "Could not get requested post", 400);

    public async Task<PagedResponse<List<Post>?>> GetFeaturedAsync(GetFeaturedPostsRequest request)
        => await _client.GetFromJsonAsync<PagedResponse<List<Post>>?>("v1/posts/featured")
            ?? new PagedResponse<List<Post>>(null, "Could not fetch posts", 400);
 

    public async Task<Response<Post?>> UpdateAsync(UpdatePostRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/posts/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Post>?>()
            ?? new Response<Post?>(null, "Could not update requested post", 400);
    }
}