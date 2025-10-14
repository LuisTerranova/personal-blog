using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface IPostHandler
{
    Task<Response<Post?>> CreateAsync(CreatePostRequest request);
    Task<Response<Post?>> DeleteAsync(DeletePostRequest request);
    Task<PagedResponse<List<Post>?>> GetAllAsync(GetAllPostsRequest request);
    Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request);
    Task<PagedResponse<List<Post>?>> GetFeaturedAsync(GetFeaturedPostsRequest request);
    Task<Response<Post?>> UpdateAsync(UpdatePostRequest request);
}