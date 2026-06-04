using TrashTechHub.Core.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TrashTechHub.Web.Components.Admin.Dashboard.Forms;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Posts;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Web.Components.Admin.Dashboard;

public partial class PostsManager
{
    #region Services
    
    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public IPostService Service { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    #endregion
    
    #region Properties
    
    private MudTable<PostDto> _table = null!;
    private string _searchString = string.Empty;
    private string SearchString
    {
        get => _searchString;
        set
        {
            _searchString = value;
            _table.ReloadServerData();
        }
    }
    private string? _errorMessage;
    
    #endregion
    
    #region Methods

    private async Task<TableData<PostDto>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllPostsRequest()
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize,
                Query = _searchString
            };

            var result = await Service.GetAllAsync(request);
            return new TableData<PostDto>() { TotalItems = result.TotalCount, Items = result.Items };
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<PostDto> { TotalItems = 0, Items = new List<PostDto>() };
    }

    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();
        
        if (id.HasValue)
        {
            var post = await Service.GetByIdAsync(new GetPostByIdRequest() { Id = id.Value });

            if (post != null)
            {
                var updateModel = new UpdatePostRequest()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    CategoryId = post.CategoryId,
                    IsFeatured = post.IsFeatured
                };
                
                parameters.Add("InitialUpdateModel", updateModel);
            }
            else
            {
                Snackbar.Add("Post data not found for editing.", Severity.Error);
                return; 
            }
        }
        
        var dialog = await DialogService.ShowAsync<PostForm>("Create/Update Post", parameters);
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
            if (id.HasValue) 
            {
                var updatePostRequest = (UpdatePostRequest)result.Data;
                var updated = await Service.UpdateAsync(updatePostRequest);
                if (updated != null)
                {
                    Snackbar.Add("Post updated successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add("Update failed with unknown error", Severity.Error);
                }
            }
            else 
            {
                var newPostRequest = (CreatePostRequest)result.Data;
                await Service.CreateAsync(newPostRequest);
                Snackbar.Add("Post created successfully!", Severity.Success);
                await _table.ReloadServerData();
            }
        }
        catch (InvalidCastException)
        {
            Snackbar.Add("Error: Data returned from the form had an incorrect type.", Severity.Error);
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
            "Do you really want to delete this post? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeletePostRequest() { Id = id };
                var deleted = await Service.DeleteAsync(request);
                if (deleted)
                {
                    Snackbar.Add("Post deleted successfully", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add("Deletion failed with unknown error", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"An unexpected error occurred while deleting the post: {ex.Message}", Severity.Error);
            }
        }
    }

    #endregion
}