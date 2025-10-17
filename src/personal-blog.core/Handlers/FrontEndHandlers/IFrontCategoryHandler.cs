using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers.FrontEndHandlers;

public interface IFrontCategoryHandler
{
    Task<PagedResponse<List<Category>?>> GetAllAsync(GetAllCategoriesRequest request);
    Task<Response<Category?>> GetByIdAsync(GetCategoryByIdRequest request);
}