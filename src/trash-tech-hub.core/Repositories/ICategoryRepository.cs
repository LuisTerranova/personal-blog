using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Categories;

namespace TrashTechHub.Core.Repositories;

public interface ICategoryRepository
{
    Task<(List<Category> Categories, int TotalCount)> GetAllAsync(GetAllCategoriesRequest request);
    Task<Category?> GetByIdAsync(int id);
    Task<bool> ExistsBySlugAsync(string slug, int? excludeId = null);
    Task<Category> CreateAsync(Category category);
    Task<Category> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
}
