using System.Security.Claims;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface ICategoryHandler
{
    Task<Response<Category?>> CreateAsync(CreateCategoryRequest request);
    Task<Response<Category?>> DeleteAsync(DeleteCategoryRequest request);
    Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request);
    Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request);
    Task<Response<Category?>> UpdateAsync(UpdateCategoryRequest request);
}