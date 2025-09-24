using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.core.Common.Helpers;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.Api.Handlers;

public class CategoryHandler(AppDbContext context) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request)
    {
        try
        {
           var slug = SlugGenHelper.GenerateSlug(request.Title);
           
           var category = new Category
           {
               UserId = request.UserId,
               Title = request.Title,
               Slug = slug
           };
           
           if (context.Categories.Any(c => c.Slug == slug))
               return new Response<Category?>(null, "Category already exists", 409);
           
           await context.Categories.AddAsync(category);
           await context.SaveChangesAsync();
           return new Response<Category?>(category, "Category created successfully", 201);
        }
        catch
        {
            return new Response<Category?>(null, "Error creating category", 400);
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request)
    {
        try
        {
            var userId = request.UserId;
            var category = await context.Categories
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync(c => c.Id == request.Id);

            if (category == null)
                return new Response<Category?>(null, "Category not found", 404);

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return new Response<Category?>(category, "Category deleted successfully");
        }
        catch
        {
            return new Response<Category?>(null, "Error deleting category", 400);
        }
    }

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request)
    {
        try
        {
            var query = context.Categories.AsNoTracking();
            
            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(c => c.Title.Contains(request.Query));
            }
            
            var totalCount = await context.Categories.AsNoTracking().CountAsync();
            
            var categories = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            
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
            var userId = request.UserId;
            var category = await context.Categories
                .Where(c => c.UserId == userId)
                .FirstOrDefaultAsync(c => c.Id == request.Id);
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