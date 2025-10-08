using Microsoft.AspNetCore.Components.Forms;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface IProjectHandler
{
    Task<Response<Project?>> CreateAsync(CreateProjectRequest request);
    Task<Response<Project?>> DeleteAsync(DeleteProjectRequest request);
    Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request);
    Task<Response<Project?>> GetByIdAsync(GetProjectByIdRequest request);
    Task<Response<Project?>> UpdateAsync(UpdateProjectRequest request);
    Task<Response<string>> UploadImageAsync(IBrowserFile file);
}