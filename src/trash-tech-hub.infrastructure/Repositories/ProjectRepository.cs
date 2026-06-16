using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Projects;
using TrashTechHub.Infrastructure.Data;

namespace TrashTechHub.Infrastructure.Repositories;

public class ProjectRepository(AppDbContext context, ILogger<ProjectRepository> logger) : IProjectRepository
{
    public async Task<(List<Project> Projects, int TotalCount)> GetAllAsync(GetAllProjectsRequest request)
    {
        var query = context.Projects.AsNoTracking();

        if (!string.IsNullOrEmpty(request.Query))
        {
            if (context.Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
            {
                query = query.Where(p => p.Title.ToLower().Contains(request.Query.ToLower()));
            }
            else
            {
                query = query.Where(p => EF.Functions.ILike(p.Title, $"%{request.Query}%"));
            }
        }

        var totalCount = await query.CountAsync();

        var projects = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (projects, totalCount);
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Project?> GetBySlugAsync(string slug)
    {
        return await context.Projects
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<Project> CreateAsync(Project project)
    {
        context.Projects.Add(project);
        return project;
    }

    public Task<Project> UpdateAsync(Project project)
    {
        context.Projects.Update(project);
        return Task.FromResult(project);
    }

    public async Task<(List<Project> Projects, int TotalCount)> GetFeaturedAsync(int pageSize)
    {
        var query = context.Projects
            .AsNoTracking()
            .Where(p => p.IsFeatured)
            .OrderByDescending(p => p.Created);

        var totalCount = await query.CountAsync();

        var featured = await query.Take(pageSize).ToListAsync();

        if (featured.Count == 0)
        {
            var fallbackQuery = context.Projects
                .AsNoTracking()
                .OrderByDescending(p => p.Created);

            totalCount = await fallbackQuery.CountAsync();
            featured = await fallbackQuery.Take(pageSize).ToListAsync();
        }

        return (featured, totalCount);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await context.Projects.FirstOrDefaultAsync(p => p.Id == id);
        if (project == null)
        {
            logger.LogWarning("Project not found for deletion: {Id}", id);
            return false;
        }

        context.Projects.Remove(project);
        return true;
    }
}
