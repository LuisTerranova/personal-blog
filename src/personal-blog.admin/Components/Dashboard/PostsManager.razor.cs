using Microsoft.AspNetCore.Components;
using MudBlazor;
using personal_blog.admin.Components.Dashboard.Forms;
using personal_blog.core.DTOs;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;

namespace personal_blog.admin.Components.Dashboard;

public partial class PostsManager
{
    #region Services
    
    [Inject]
    public ISnackbar? Snackbar { get; set; }
    [Inject]
    public IPostHandler? Handler { get; set; }
    [Inject]
    private IDialogService? DialogService { get; set; }
    
    

    #endregion
    
    #region Properties
    
    private MudTable<PostDTO>? _table;
    private string? _searchString;
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

    private async Task<TableData<PostDTO>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllPostsRequest()
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize,
                Query = _searchString
            };

            var result = await Handler.GetAllAsync(request);

            if (result.IsSuccess)
            {
                return new TableData<PostDTO>()
                {
                    TotalItems = result.TotalCount,
                    Items = result.Data ?? new List<PostDTO>()
                };
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<PostDTO> { TotalItems = 0, Items = new List<PostDTO>() };
    }
    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();
        
        if (id.HasValue)
        {
            var getResult = await Handler.GetByIdAsync(new GetPostByIdRequest() { Id = id.Value });

            if (getResult.IsSuccess && getResult.Data != null)
            {
                var updateModel = new UpdatePostRequest()
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title,
                    Body = getResult.Data.Body,
                    CategoryId =  getResult.Data.CategoryId
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
        
        if (result.Canceled || result.Data == null)
            return; 
        
        try
        {
            if (id.HasValue) 
            {
                var updatePostRequest = (UpdatePostRequest)result.Data;
                var updateResult = await Handler.UpdateAsync(updatePostRequest);
            
                if (updateResult.IsSuccess)
                {
                    Snackbar.Add("Post updated successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(updateResult.Message, Severity.Error);
                }
            }
            else 
            {
                var newPostRequest = (CreatePostRequest)result.Data;
                var createResult = await Handler.CreateAsync(newPostRequest);

                if (createResult.IsSuccess && createResult.Data is not null)
                {
                    Snackbar.Add("Post created successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(createResult.Message, Severity.Error);
                }
            }
        }
        catch (InvalidCastException)
        {
            Snackbar.Add("Error: Data returned from the form had an incorrect type. Check the form's Update/Create methods.", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
        }
    }

    private async Task Delete(int id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning",
            "Do you really want to delete this post? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeletePostRequest()
                {
                    Id = id
                };
                
                var deleteResult = await Handler.DeleteAsync(request);

                if (deleteResult.IsSuccess)
                {
                    Snackbar.Add(deleteResult.Message, Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(deleteResult.Message, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add("An unexpected error occurred while deleting the post.", Severity.Error);
            }
        }
    }

    #endregion
}