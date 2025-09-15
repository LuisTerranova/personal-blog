using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.core.Common.Helpers;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
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

    public async Task<PagedResponse<List<Post>?>> GetAllAsync(GetAllPostsRequest request)
    {
        try
        {
            var posts = await context.Posts
                .AsNoTracking()
                .Skip(request.PageNumber - 1)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = posts
                .Count;
            return totalCount == 0
                ? new PagedResponse<List<Post>?>(null, "Posts not found", 400)
                : new PagedResponse<List<Post>?>(posts, totalCount);
        }
        catch 
        {
            return new PagedResponse<List<Post>?>(null, "Error while retrieving posts", 400);
        }
    }
    
    public async Task<Response<Post?>> GetByIdAsync(GetPostByIdRequest request)
    {
        try
        {
            var post = await context.Posts
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

    public async Task<PagedResponse<List<Post>?>> GetFeaturedAsync(GetFeaturedPostsRequest request)
    {
        try
        {
            var featuredPosts = await context.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.Created)
                .Take(3)
                .ToListAsync();
            
            return featuredPosts == null
                ? new PagedResponse<List<Post>?>(null, "Posts not found", 404)
                : new PagedResponse<List<Post>?>(featuredPosts, "Posts retrieved successfully");
        }
        catch
        {
            return new PagedResponse<List<Post>?>(null, "Error while retrieving posts", 400);
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