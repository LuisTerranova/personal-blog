using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using personal_blog.core.Handlers;
using personal_blog.core.Handlers.FrontEndHandlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.front.Handlers;

public class ProjectHandler(IHttpClientFactory httpClientFactory) : IFrontProjectHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public async Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request)
    {
        return await _client.GetFromJsonAsync<PagedResponse<List<Project>>?>("v1/projects")
               ?? new PagedResponse<List<Project>>(null, "Could not fetch projects", 400);
    }
    public async Task<Response<Project?>> GetByIdAsync(GetProjectByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Project?>>($"v1/projects/{request.Id}")
           ?? new Response<Project?>(null, "Could not get requested project", 400);
}