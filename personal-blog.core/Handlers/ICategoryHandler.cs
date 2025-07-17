using System.Security.Claims;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface ICategoryHandler
{
    Task<Response<Category?>> CreateAsync(CreateCategoryRequest request, ClaimsPrincipal user);
    Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request, ClaimsPrincipal user);
    Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request, ClaimsPrincipal user);
    Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request, ClaimsPrincipal user);
    Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request, ClaimsPrincipal user);
}