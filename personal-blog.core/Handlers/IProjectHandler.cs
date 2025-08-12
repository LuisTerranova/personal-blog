using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers;

public interface IProjectHandler
{
    Task<Response<Project?>> CreateAsync(CreateProjectRequest request);
    Task<Response<Project?>> DeleteAsync(DeleteProjectRequest request);
    Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request);
    Task<Response<Project?>> UpdateAsync(UpdateProjectRequest request);
}