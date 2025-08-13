using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.core.Responses;

namespace personal_blog.Api.Handlers;

public class ProjectHandler(AppDbContext context) : IProjectHandler
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

            if (project == null)
                return new Response<Project?>(null, "Project not found", 404);

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
}