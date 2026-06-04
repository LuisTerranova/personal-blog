using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Posts;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Infrastructure.Repositories;

public class PostRepository(AppDbContext context, ILogger<PostRepository> logger) : IPostRepository
{
    public async Task<(List<Post> Posts, int TotalCount)> GetAllAsync(GetAllPostsRequest request)
    {
        var query = context.Posts.Include(p => p.Category).AsNoTracking();

        if (!string.IsNullOrEmpty(request.Query))
        {
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                query = query.Where(p => p.Title.ToLower().Contains(request.Query.ToLower()));
            }
            else
            {
                query = query.Where(p => EF.Functions.ILike(p.Title, $"%{request.Query}%"));
            }
        }

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId.Value);

        var totalCount = await query.CountAsync();

        var posts = await query
            .OrderByDescending(p => p.Created)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (posts, totalCount);
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        return await context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        return await context.Posts
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<Post> CreateAsync(Post post)
    {
        context.Posts.Add(post);
        return post;
    }

    public Task<Post> UpdateAsync(Post post)
    {
        context.Posts.Update(post);
        return Task.FromResult(post);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            logger.LogWarning("Post not found for deletion: {Id}", id);
            return false;
        }

        context.Posts.Remove(post);
        return true;
    }

    public async Task<(List<Post> Posts, int TotalCount)> GetFeaturedAsync(int pageSize)
    {
        var query = context.Posts
            .Include(p => p.Category)
            .AsNoTracking()
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.Created);

        var totalCount = await query.CountAsync();

        var featured = await query.Take(pageSize).ToListAsync();

        if (featured.Count == 0)
        {
            var fallbackQuery = context.Posts
                .Include(p => p.Category)
                .AsNoTracking()
                .OrderByDescending(p => p.Created);

            totalCount = await fallbackQuery.CountAsync();
            featured = await fallbackQuery.Take(pageSize).ToListAsync();
        }

        return (featured, totalCount);
    }
}
