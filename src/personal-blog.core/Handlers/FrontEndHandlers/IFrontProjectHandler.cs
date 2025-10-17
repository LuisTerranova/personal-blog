using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.core.Handlers.FrontEndHandlers;

public interface IFrontProjectHandler
{
    Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request);
    Task<Response<Project?>> GetByIdAsync(GetProjectByIdRequest request);
}