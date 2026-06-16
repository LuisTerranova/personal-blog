using TrashTechHub.Core.Common;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Projects;

namespace TrashTechHub.Core.Services;

public interface IProjectService
{
    Task<PagedResult<ProjectDto>> GetAllAsync(GetAllProjectsRequest request);
    Task<ProjectDto?> GetByIdAsync(GetProjectByIdRequest request);
    Task<ProjectDto?> GetBySlugAsync(string slug);
    Task<ProjectDto> CreateAsync(CreateProjectRequest request);
    Task<ProjectDto?> UpdateAsync(UpdateProjectRequest request);
    Task<bool> DeleteAsync(DeleteProjectRequest request);
    Task<PagedResult<ProjectDto>> GetFeaturedAsync(GetFeaturedProjectsRequest request);
}
