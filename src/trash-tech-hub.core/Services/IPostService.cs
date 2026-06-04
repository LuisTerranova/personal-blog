using TrashTechHub.Core.Common;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Posts;

namespace TrashTechHub.Core.Services;

public interface IPostService
{
    Task<PagedResult<PostDto>> GetAllAsync(GetAllPostsRequest request);
    Task<PostDto?> GetByIdAsync(GetPostByIdRequest request);
    Task<PostDto?> GetBySlugAsync(string slug);
    Task<PostDto> CreateAsync(CreatePostRequest request);
    Task<PostDto?> UpdateAsync(UpdatePostRequest request);
    Task<bool> DeleteAsync(DeletePostRequest request);
    Task<PagedResult<PostDto>> GetFeaturedAsync(GetFeaturedPostsRequest request);
}
