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
                CategoryId = request.CategoryId
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
                .FirstOrDefaultAsync(c => c.Id == request.Id);

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

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var categories = await context.Categories
                .AsNoTracking()
                .Skip(request.PageNumber - 1)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = categories.Count;
            return totalCount == 0
                ? new PagedResponse<List<Category>?>(null, "Categories not found", 400)
                : new PagedResponse<List<Category>?>(categories, totalCount);
        }
        catch 
        {
            return new PagedResponse<List<Category>?>(null, "Error while retrieving categories", 400);
        }
    }

    public async Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request)
    {
        try
        {
            var category = await context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            return category == null
                ? new Response<Category?>(null, "Category not found", 404)
                : new Response<Category?>(category, "Category retrieved successfully");
        }
        catch
        {
            return new Response<Category?>(null, "Error retrieving category", 400 );
        }
    }

    public async Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);
            if (category == null)
                return new Response<Category?>(null, "Category not found", 404);

            category.Title = request.Title;
            category.Slug = SlugGenHelper.GenerateSlug(request.Title);

            await context.SaveChangesAsync();
            return new Response<Category?>(category, "Category updated successfully");
        }
        catch
        {
            return new Response<Category?>(null, "Error updating category", 400);
        }
    }
}