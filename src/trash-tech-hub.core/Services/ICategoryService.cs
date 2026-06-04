using TrashTechHub.Core.Common;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Categories;

namespace TrashTechHub.Core.Services;

public interface ICategoryService
{
    Task<PagedResult<CategoryDto>> GetAllAsync(GetAllCategoriesRequest request);
    Task<CategoryDto?> GetByIdAsync(GetCategoryByIdRequest request);
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task<CategoryDto?> UpdateAsync(UpdateCategoryRequest request);
    Task<bool> DeleteAsync(DeleteCategoryRequest request);
}
