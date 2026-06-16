using TrashTechHub.Core.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TrashTechHub.Core.Common.Helpers;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Projects;
using TrashTechHub.Core.Services;
using TrashTechHub.Web.Components.Admin.Dashboard.Forms;

namespace TrashTechHub.Web.Components.Admin.Dashboard;

public partial class ProjectsManager
{
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IProjectService Service { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    
    #endregion
    
    #region Properties
    
    private MudTable<ProjectDto> _table = null!;
    private string? _errorMessage;
    
    #endregion
    
    #region Methods

    private async Task<TableData<ProjectDto>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllProjectsRequest()
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize
            };

            var result = await Service.GetAllAsync(request);
            return new TableData<ProjectDto>() { TotalItems = result.TotalCount, Items = result.Items };
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<ProjectDto> { TotalItems = 0, Items = new List<ProjectDto>() };
    }

    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();

        if (id.HasValue)
        {
            var project = await Service.GetByIdAsync(new GetProjectByIdRequest() { Id = id.Value });

            if (project != null)
            {
                var updateModel = new UpdateProjectRequest()
                {
                    Id = project.Id,
                    Title = project.Title,
                    Description = project.Description,
                    Summary = project.Summary,
                    ImageUrl = project.ImageUrl,
                    RepoLink = project.RepoLink,
                    IsFeatured = project.IsFeatured
                };

                parameters.Add("InitialUpdateModel", updateModel);
            }
            else
            {
                Snackbar.Add("Project data not found for editing.", Severity.Error);
                return;
            }
        }

        var dialog = await DialogService.ShowAsync<ProjectForm>("Create/Update Project", parameters);
        var result = await dialog.Result;

        if (result?.Data is null)
        {
            Snackbar.Add("Form submission failed. Data was not returned correctly.", Severity.Error);
            return;
        }
        
        if (result.Canceled)
            return;

        try
        {
            var projectRequest = (ProjectRequestWithImageBase)result.Data;

            if (id.HasValue)
            {
                await Service.UpdateAsync((UpdateProjectRequest)projectRequest);
                Snackbar.Add("Project updated successfully!", Severity.Success);
                await _table.ReloadServerData();
            }
            else
            {
                await Service.CreateAsync((CreateProjectRequest)projectRequest);
                Snackbar.Add("Project created successfully!", Severity.Success);
                await _table.ReloadServerData();
            }
        }
        catch (InvalidCastException)
        {
            Snackbar.Add(
                "Error: Data returned from the form had an incorrect type. Check the form's Update/Create methods.",
                Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
        }
    }

    private async Task Delete(int id)
    {
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Warning",
            "Do you really want to delete this project? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeleteProjectRequest() { Id = id };
                var deleted = await Service.DeleteAsync(request);
                if (deleted)
                {
                    Snackbar.Add("Project deleted successfully", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add("Deletion failed with unknown error", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
            }
        }
    }

    #endregion
}
