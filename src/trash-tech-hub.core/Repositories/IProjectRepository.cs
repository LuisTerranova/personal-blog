using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Projects;

namespace TrashTechHub.Core.Repositories;

public interface IProjectRepository
{
    Task<(List<Project> Projects, int TotalCount)> GetAllAsync(GetAllProjectsRequest request);
    Task<Project?> GetByIdAsync(int id);
    Task<Project?> GetBySlugAsync(string slug);
    Task<Project> CreateAsync(Project project);
    Task<Project> UpdateAsync(Project project);
    Task<bool> DeleteAsync(int id);
    Task<(List<Project> Projects, int TotalCount)> GetFeaturedAsync(int pageSize);
}
