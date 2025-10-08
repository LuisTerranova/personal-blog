using Microsoft.AspNetCore.Components;
using MudBlazor;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Projects;
using personal_blog.front.Components.Dashboard.Forms;

namespace personal_blog.front.Components.Dashboard;

public partial class ProjectsManager
{
    #region Services
    
    [Inject]
    public ISnackbar snackbar { get; set; }
    [Inject]
    public IProjectHandler Handler { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    
    

    #endregion
    
    #region Properties
    
    private MudTable<Project> _table;
    private string? _errorMessage;
    
    #endregion
    
    #region Methods

    private async Task<TableData<Project>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllProjectsRequest()
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize
            };

            var result = await Handler.GetAllAsync(request);

            if (result.IsSuccess)
            {
                return new TableData<Project>()
                {
                    TotalItems = result.TotalCount,
                    Items = result.Data ?? new List<Project>()
                };
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<Project> { TotalItems = 0, Items = new List<Project>() };
    }

    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();

        if (id.HasValue)
        {
            var getResult = await Handler.GetByIdAsync(new GetProjectByIdRequest() { Id = id.Value });

            if (getResult.IsSuccess && getResult.Data != null)
            {
                var updateModel = new UpdateProjectRequest()
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    Description = getResult.Data.Description,
                    ImageUrl = getResult.Data.ImageUrl,
                    RepoLink = getResult.Data.RepoLink
                };

                parameters.Add("InitialUpdateModel", updateModel);
            }
            else
            {
                snackbar.Add("Project data not found for editing.", Severity.Error);
                return;
            }
        }

        var dialog = DialogService.Show<ProjectForm>("Create/Update Project", parameters);
        var result = await dialog.Result;

        if (result.Canceled || result.Data == null)
            return;

        try
        {
            if (id.HasValue)
            {
                var updateProjectRequest = (UpdateProjectRequest)result.Data;

                if (updateProjectRequest.ImageFile is not null)
                {
                    var uploadResult = await Handler.UploadImageAsync(updateProjectRequest.ImageFile);

                    if (uploadResult.IsSuccess && !string.IsNullOrEmpty(uploadResult.Data))
                    {
                        updateProjectRequest.ImageUrl = uploadResult.Data;
                    }
                    else
                    {
                        snackbar.Add($"Image upload failed: {uploadResult.Message}. Project update cancelled.",
                            Severity.Error);
                        return;
                    }
                }

                var updateResult = await Handler.UpdateAsync(updateProjectRequest);

                if (updateResult.IsSuccess)
                {
                    snackbar.Add("Project updated successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    snackbar.Add(updateResult.Message, Severity.Error);
                }
            }
            else
            {
                var newProjectRequest = (CreateProjectRequest)result.Data;

                if (newProjectRequest.ImageFile is not null)
                {
                    var uploadResult = await Handler.UploadImageAsync(newProjectRequest.ImageFile);

                    if (uploadResult.IsSuccess && !string.IsNullOrEmpty(uploadResult.Data))
                    {
                        newProjectRequest.ImageUrl = uploadResult.Data;
                    }
                    else
                    {
                        snackbar.Add($"Image upload failed: {uploadResult.Message}. Project creation cancelled.",
                            Severity.Error);
                        return;
                    }
                }

                var createResult = await Handler.CreateAsync(newProjectRequest);

                if (createResult.IsSuccess && createResult.Data is not null)
                {
                    snackbar.Add("Project created successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    snackbar.Add(createResult.Message, Severity.Error);
                }
            }
        }
        catch (InvalidCastException)
        {
            snackbar.Add(
                "Error: Data returned from the form had an incorrect type. Check the form's Update/Create methods.",
                Severity.Error);
        }
        catch (Exception ex)
        {
            snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
        }
    }

    private async Task Delete(int id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning",
            "Do you really want to delete this project? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeleteProjectRequest()
                {
                    Id = id
                };
                
                var deleteResult = await Handler.DeleteAsync(request);

                if (deleteResult.IsSuccess)
                {
                    snackbar.Add(deleteResult.Message, Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    snackbar.Add(deleteResult.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                snackbar.Add("An unexpected error occurred while deleting the project.", Severity.Error);
            }
        }
    }

    #endregion
}