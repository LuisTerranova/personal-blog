using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Posts;

namespace TrashTechHub.Core.Repositories;

public interface IPostRepository
{
    Task<(List<Post> Posts, int TotalCount)> GetAllAsync(GetAllPostsRequest request);
    Task<Post?> GetByIdAsync(int id);
    Task<Post?> GetBySlugAsync(string slug);
    Task<Post> CreateAsync(Post post);
    Task<Post> UpdateAsync(Post post);
    Task<bool> DeleteAsync(int id);
    Task<(List<Post> Posts, int TotalCount)> GetFeaturedAsync(int pageSize);
}
