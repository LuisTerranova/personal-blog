using Microsoft.Extensions.Logging;
using AutoMapper;
using TrashTechHub.Core.Common;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Projects;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Application.Services;

public class ProjectService(
    IProjectRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IFileStorageService fileStorageService,
    ILogger<ProjectService> logger) : IProjectService
{
    public async Task<PagedResult<ProjectDto>> GetAllAsync(GetAllProjectsRequest request)
    {
        var (projects, totalCount) = await repository.GetAllAsync(request);
        return new PagedResult<ProjectDto>
        {
            Items = mapper.Map<List<ProjectDto>>(projects),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<ProjectDto?> GetByIdAsync(GetProjectByIdRequest request)
    {
        var project = await repository.GetByIdAsync(request.Id);
        return mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto?> GetBySlugAsync(string slug)
    {
        var project = await repository.GetBySlugAsync(slug);
        return mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectRequest request)
    {
        var imageUrl = string.Empty;
        if (request.ImageFile != null)
        {
            await using var stream = request.ImageFile.OpenReadStream(maxAllowedSize: 512000);
            imageUrl = await fileStorageService.SaveFileAsync(stream, request.ImageFile.Name, "images/projects");
        }

        var project = new Project
        {
            Title = request.Title,
            Description = request.Description,
            Summary = request.Summary,
            ImageUrl = imageUrl,
            RepoLink = request.RepoLink,
            Slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : request.Slug
        };

        await repository.CreateAsync(project);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Created project: {Id} ({Title})", project.Id, project.Title);

        return mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto?> UpdateAsync(UpdateProjectRequest request)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing == null) return null;

        var imageUrl = existing.ImageUrl;
        if (request.ImageFile != null)
        {
            await using var stream = request.ImageFile.OpenReadStream(maxAllowedSize: 512000);
            imageUrl = await fileStorageService.SaveFileAsync(stream, request.ImageFile.Name, "images/projects");

            if (!string.IsNullOrEmpty(existing.ImageUrl))
                await fileStorageService.DeleteFileAsync(existing.ImageUrl);
        }

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.Summary = request.Summary;
        existing.ImageUrl = imageUrl;
        existing.RepoLink = request.RepoLink;
        existing.Slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : request.Slug;

        await repository.UpdateAsync(existing);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Updated project: {Id}", existing.Id);

        return mapper.Map<ProjectDto>(existing);
    }

    private string GenerateSlug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Guid.NewGuid().ToString("n")[..8];
        var slug = title.ToLowerInvariant();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }

    public async Task<bool> DeleteAsync(DeleteProjectRequest request)
    {
        var project = await repository.GetByIdAsync(request.Id);
        if (project == null) return false;

        var imageUrl = project.ImageUrl;

        var result = await repository.DeleteAsync(request.Id);
        if (result)
        {
            await unitOfWork.SaveChangesAsync();
            if (!string.IsNullOrEmpty(imageUrl))
            {
                await fileStorageService.DeleteFileAsync(imageUrl);
            }
            logger.LogInformation("Deleted project: {Id}", request.Id);
        }
        return result;
    }
}
