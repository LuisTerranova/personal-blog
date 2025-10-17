using personal_blog.core.DTOs;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers.FrontEndHandlers;

public interface IFrontPostHandler
{
    Task<PagedResponse<List<PostDTO>?>> GetAllAsync(GetAllPostsRequest request);
    Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request);
    Task<PagedResponse<List<PostDTO>?>> GetFeaturedAsync(GetFeaturedPostsRequest request);
}