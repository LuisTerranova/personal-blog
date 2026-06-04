using Microsoft.Extensions.Logging;
using AutoMapper;
using TrashTechHub.Core.Common;
using TrashTechHub.Core.Common.Helpers;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Categories;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Application.Services;

public class CategoryService(
    ICategoryRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<CategoryService> logger) : ICategoryService
{
    public async Task<PagedResult<CategoryDto>> GetAllAsync(GetAllCategoriesRequest request)
    {
        var (categories, totalCount) = await repository.GetAllAsync(request);
        return new PagedResult<CategoryDto>
        {
            Items = mapper.Map<List<CategoryDto>>(categories),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<CategoryDto?> GetByIdAsync(GetCategoryByIdRequest request)
    {
        var category = await repository.GetByIdAsync(request.Id);
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        var slug = SlugGenHelper.GenerateSlug(request.Title);

        if (await repository.ExistsBySlugAsync(slug))
        {
            logger.LogWarning("Slug conflict on create: {Slug}", slug);
            throw new InvalidOperationException($"Category with slug '{slug}' already exists");
        }

        var category = new Category
        {
            Title = request.Title,
            Slug = slug
        };

        await repository.CreateAsync(category);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Created category: {Id} ({Title})", category.Id, category.Title);
        
        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateAsync(UpdateCategoryRequest request)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing == null) return null;

        var slug = SlugGenHelper.GenerateSlug(request.Title);
        if (await repository.ExistsBySlugAsync(slug, request.Id))
        {
            logger.LogWarning("Slug conflict on update: {Slug} (excluding category {Id})", slug, request.Id);
            throw new InvalidOperationException($"Category with slug '{slug}' already exists");
        }

        existing.Title = request.Title;
        existing.Slug = slug;

        await repository.UpdateAsync(existing);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Updated category: {Id}", existing.Id);
        
        return mapper.Map<CategoryDto>(existing);
    }

    public async Task<bool> DeleteAsync(DeleteCategoryRequest request)
    {
        var result = await repository.DeleteAsync(request.Id);
        if (result)
        {
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Deleted category: {Id}", request.Id);
        }
        return result;
    }
}
