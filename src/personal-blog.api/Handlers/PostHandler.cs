using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.core.DTOs;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.Api.Handlers;

public class PostHandler(AppDbContext context) : IPostHandler
{
    public async Task<Response<Post?>> CreateAsync(CreatePostRequest request)
    {
        try
        {
           var post = new Post
           {
                Title = request.Title,
                Body = request.Body,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
           };

           await context.Posts.AddAsync(post);
           await context.SaveChangesAsync();
           
           return new Response<Post?>(post, "Post created successfully", 201);
        }
        catch
        {
            return new Response<Post?>(null, "Error creating post", 400);
        }
    }

    public async Task<Response<Post?>> DeleteAsync(DeletePostRequest request)
    {
        try
        {
            var post = await context.Posts
                .FirstOrDefaultAsync(p => p.Id == request.Id
                && p.UserId == request.UserId);

            if (post == null)
                return new Response<Post?>(null, "Post not found", 404);

            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            return new Response<Post?>(post, "Post deleted successfully");
        }
        catch
        {
            return new Response<Post?>(null, "Error deleting post", 400);
        }
    }

    public async Task<PagedResponse<List<PostDTO>?>> GetAllAsync(GetAllPostsRequest request)
    {
        var maxLength = 200;
        try
        {
            var query = context.Posts.AsNoTracking();
            
            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(c => c.Title.Contains(request.Query));
            }
            
            var totalCount = await query.CountAsync();

            var posts = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Summary = Regex.Replace(p.Body, "<.*?>", string.Empty).Length > maxLength
                        ? Regex.Replace(p.Body, "<.*?>", string.Empty).Substring(0, maxLength) + "..."
                        : Regex.Replace(p.Body, "<.*?>", string.Empty),
                    Category = p.Category,
                    Created = p.Created,
                    Updated = p.Updated
                })
                .ToListAsync();
            
            return totalCount == 0
                ? new PagedResponse<List<PostDTO>?>(null, "Posts not found")
                : new PagedResponse<List<PostDTO>?>(posts, totalCount);
        }
        catch 
        {
            return new PagedResponse<List<PostDTO>?>(null, "Error while retrieving posts", 400);
        }
    }
    
    public async Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request)
    {
        try
        {
            var post = await context.Posts
                .Include(p=> p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            return post == null
                ? new Response<Post?>(null, "Post not found", 404)
                : new Response<Post?>(post, "Post retrieved successfully");
        }
        catch
        {
            return new Response<Post?>(null, "Error retrieving post", 400 );
        }
    }

    public async Task<PagedResponse<List<PostDTO>?>> GetFeaturedAsync(GetFeaturedPostsRequest request)
    {
        try
        {
            var maxLength = 200;
            var featuredPosts = await context.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.Created)
                .Take(3)
                .Select(p=> new PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Summary = Regex.Replace(p.Body, "<.*?>", string.Empty).Length > maxLength
                        ? Regex.Replace(p.Body, "<.*?>", string.Empty).Substring(0, maxLength) + "..."
                        : Regex.Replace(p.Body, "<.*?>", string.Empty),
                    Category = p.Category,
                    Created = p.Created
                })
                .ToListAsync();
            
            return featuredPosts == null
                ? new PagedResponse<List<PostDTO>?>(null, "Posts not found", 404)
                : new PagedResponse<List<PostDTO>?>(featuredPosts, "Posts retrieved successfully");
        }
        catch
        {
            return new PagedResponse<List<PostDTO>?>(null, "Error while retrieving posts", 400);
        }
    }

    public async Task<Response<Post?>> UpdateAsync(UpdatePostRequest request)
    {
        try
        {
            var post = await context.Posts
                .FirstOrDefaultAsync(p => p.Id == request.Id
                && p.UserId == request.UserId);
            if (post == null)
                return new Response<Post?>(null, "Post not found", 404);

            post.Title = request.Title;
            post.Body = request.Body;
            post.CategoryId = request.CategoryId;

            await context.SaveChangesAsync();
            return new Response<Post?>(post, "Post updated successfully");
        }
        catch
        {
            return new Response<Post?>(null, "Error updating post", 400);
        }
    }
}