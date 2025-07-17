using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Common.Helpers;
using personal_blog.Api.Data;
using personal_blog.core.Common.Helpers;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.Api.Handlers;

public class CategoryHandler(AppDbContext context, UserManager<IdentityUser> userManager) : ICategoryHandler
{
    public async Task<Response<Category?>> CreateAsync(CreateCategoryRequest request, ClaimsPrincipal user)
    {
        try
        {
           var authResponse = await CheckAdminHelper.CheckAdminAsync(user, userManager);
           if(!authResponse.IsSuccess)
               return new Response<Category?>(null, authResponse.Message);
           
           var slug = SlugGenHelper.GenerateSlug(request.Title);
           var category = new Category
           {
                Title = request.Title,
                Slug = slug
           };

           await context.Categories.AddAsync(category);
           await context.SaveChangesAsync();
           return new Response<Category?>(category, "Category created successfully", 201);
        }
        catch
        {
            return new Response<Category?>(null, "Error creating category", 400);
        }
    }

    public async Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request, ClaimsPrincipal user)
    {
        try
        {
            var authResponse = await CheckAdminHelper.CheckAdminAsync(user, userManager);
            if (!authResponse.IsSuccess)
                return new Response<Category?>(null, authResponse.Message);

            var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id);

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

    public async Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request, ClaimsPrincipal user)
    {
        try
        {
            var authResponse = await CheckAdminHelper.CheckAdminAsync(user, userManager);
            if (!authResponse.IsSuccess)
                return new PagedResponse<List<Category>?>(null, authResponse.Message);

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

    public Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }
}