using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Categories;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
{
    public async Task<(List<Category> Categories, int TotalCount)> GetAllAsync(GetAllCategoriesRequest request)
    {
        var query = context.Categories.AsNoTracking();

        if (!string.IsNullOrEmpty(request.Query))
        {
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                query = query.Where(c => c.Title.ToLower().Contains(request.Query.ToLower()));
            }
            else
            {
                query = query.Where(c => EF.Functions.ILike(c.Title, $"%{request.Query}%"));
            }
        }

        var totalCount = await query.CountAsync();

        var categories = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (categories, totalCount);
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExistsBySlugAsync(string slug, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await context.Categories.AnyAsync(c => c.Slug == slug && c.Id != excludeId.Value);
        }
        return await context.Categories.AnyAsync(c => c.Slug == slug);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        context.Categories.Add(category);
        return category;
    }

    public Task<Category> UpdateAsync(Category category)
    {
        context.Categories.Update(category);
        return Task.FromResult(category);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        if (category == null)
        {
            logger.LogWarning("Category not found for deletion: {Id}", id);
            return false;
        }

        context.Categories.Remove(category);
        return true;
    }
}
