using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
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

    public Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category?>> GetAllAsync(GetAllCategoriesRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category?>> GetAsync(GetCategoryByIdRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request, ClaimsPrincipal user)
    {
        throw new NotImplementedException();
    }
}