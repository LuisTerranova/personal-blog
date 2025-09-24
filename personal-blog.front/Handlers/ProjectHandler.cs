using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.front.Handlers;

public class ProjectHandler(IHttpClientFactory httpClientFactory) : IProjectHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public Task<Response<Project?>> CreateAsync(CreateProjectRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Project?>> DeleteAsync(DeleteProjectRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Response<Project?>> UpdateAsync(UpdateProjectRequest request)
    {
        throw new NotImplementedException();
    }
}