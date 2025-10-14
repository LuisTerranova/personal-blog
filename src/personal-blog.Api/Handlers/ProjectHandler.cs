using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.Api.Handlers;

public class ProjectHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor) : IProjectHandler
{
    public async Task<Response<Project?>> CreateAsync(CreateProjectRequest request)
    {
        try
        {
            var project = new Project
            {
                Title = request.Title,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                RepoLink = request.RepoLink,
                UserId = request.UserId,
            };

            await context.Projects.AddAsync(project);
            await context.SaveChangesAsync();
            return new Response<Project?>(project, "Project created successfully", 201);
        }
        catch
        {
            return new Response<Project?>(null, "Error creating project", 400);
        }
    }

    public async Task<Response<Project?>> DeleteAsync(DeleteProjectRequest request)
    {
        try
        {
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id);
            var imageUrl = project.ImageUrl;
            
            if (project == null)
                return new Response<Project?>(null, "Project not found", 404);

            var fileName = Path.GetFileName(imageUrl);

            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "projects");
            var physicalPath = Path.Combine(basePath, fileName);
            
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
            
            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return new Response<Project?>(project, "Project deleted successfully");
        }
        catch
        {
            return new Response<Project?>(null, "Error deleting project", 400);
        }
    }

    public async Task<PagedResponse<List<Project>?>> GetAllAsync(GetAllProjectsRequest request)
    {
        try
        {
            var projects = await context.Projects
                .AsNoTracking()
                .Skip(request.PageNumber - 1)
                .Take(request.PageSize)
                .ToListAsync();

            var totalCount = projects
                .Count;
            return totalCount == 0
                ? new PagedResponse<List<Project>?>(null, "Projects not found", 400)
                : new PagedResponse<List<Project>?>(projects, totalCount);
        }
        catch
        {
            return new PagedResponse<List<Project>?>(null, "Error while retrieving projects", 400);
        }
    }

    public async Task<Response<Project?>> GetByIdAsync(GetProjectByIdRequest request)
    {
        try
        {
            var project = await context.Projects
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            return project == null
                ? new Response<Project?>(null, "Project not found", 404)
                : new Response<Project?>(project, "Project retrieved successfully");
        }
        catch
        {
            return new Response<Project?>(null, "Error retrieving project", 400);
        }
    }

    public async Task<Response<Project?>> UpdateAsync(UpdateProjectRequest request)
    {
        try
        {
            var project = await context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id
                                          && p.UserId == request.UserId);
            if (project == null)
                return new Response<Project?>(null, "Project not found", 404);

            project.Title = request.Title;
            project.Description = request.Description;
            project.ImageUrl = request.ImageUrl;
            project.RepoLink = request.RepoLink;

            await context.SaveChangesAsync();
            return new Response<Project?>(project, "Project updated successfully");
        }
        catch
        {
            return new Response<Project?>(null, "Error updating project", 400);
        }
    }
    
    public async Task<Response<string>> UploadImageAsync(Stream fileStream, string fileName)
    {
        try
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "projects");

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            var fileExtension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var fullPath = Path.Combine(basePath, uniqueFileName);
            
            await using var diskStream = new FileStream(fullPath, FileMode.Create);
            await fileStream.CopyToAsync(diskStream);
            var httpContext = httpContextAccessor.HttpContext;
        
            if (httpContext == null)
            {
                throw new InvalidOperationException("Não foi possível obter a URL completa");
            }
        
            var request = httpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var absoluteUrl = $"{baseUrl}/images/projects/{uniqueFileName}";
        
            return new Response<string>(absoluteUrl, "Imagem enviada com sucesso", 200);
        }
        catch (Exception ex)
        {
            return new Response<string>(null, $"Erro durante o upload: {ex.Message}", 500);
        }
    }
}