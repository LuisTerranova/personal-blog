using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface IPostHandler
{
    Task<Response<Post?>> CreateAsync(CreatePostRequest request);
    Task<Response<Post?>> DeleteAsync(DeletePostRequest request);
    Task<Response<Post?>> GetAllAsync(GetAllPostsRequest request);
    Task<Response<Post?>> GetAsync(GetPostByIdRequest request);
    Task<Response<Post?>> UpdateAsync(UpdatePostRequest request);
}