using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.admin.Handlers;

public class ProjectHandler(IHttpClientFactory httpClientFactory) : IProjectHandler
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("API");
    public async Task<Response<Project?>> CreateAsync(CreateProjectRequest request)
    {
        var result =  await _client.PostAsJsonAsync("v1/projects", request);
        return await result.Content.ReadFromJsonAsync<Response<Project?>>()
               ?? new Response<Project?>(null, "Could not create project", 400);
    }

    public async Task<Response<Project?>> DeleteAsync(DeleteProjectRequest request)
    {
        var result = await _client.DeleteAsync($"v1/projects/{request.Id}");
        return await result.Content.ReadFromJsonAsync<Response<Project?>>()
               ??  new Response<Project?>(null, "Could not delete requested project", 400);
    }

    public async Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request)
    {
        return await _client.GetFromJsonAsync<PagedResponse<List<Project>?>>("v1/projects")
               ?? new PagedResponse<List<Project>?>(null, "Could not fetch projects", 400);
    }

    public async Task<Response<Project?>> GetByIdAsync(GetProjectByIdRequest request)
        => await _client.GetFromJsonAsync<Response<Project?>>($"v1/projects/{request.Id}")
           ?? new Response<Project?>(null, "Could not get requested project", 400);

    public async Task<Response<Project?>> UpdateAsync(UpdateProjectRequest request)
    {
        var result = await _client.PutAsJsonAsync($"v1/projects/{request.Id}", request);
        return await result.Content.ReadFromJsonAsync<Response<Project?>>()
               ?? new Response<Project?>(null, "Could not update requested project", 400);
    }
    
    public async Task<Response<string>> UploadImageAsync(Stream fileStream, string fileName)
    {
        try
        {
            using var content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(fileStream);
            
            content.Add(fileContent, "file", fileName); 
            var response = await _client.PostAsync("v1/storage/upload", content);

            response.EnsureSuccessStatusCode(); 
        
            var result = await response.Content.ReadFromJsonAsync<Response<string>>();
            return result ?? new Response<string>("Server returned null URL.", null);
        }
        catch (Exception ex)
        {
            return new Response<string>($"Image upload failed: {ex.Message}", null);
        }
    }
}