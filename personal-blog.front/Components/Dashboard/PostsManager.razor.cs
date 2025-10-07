using Microsoft.AspNetCore.Components;
using MudBlazor;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.front.Components.Dashboard.Forms;

namespace personal_blog.front.Components.Dashboard;

public partial class PostsManager
{
    #region Services
    
    [Inject]
    public ISnackbar snackbar { get; set; }
    [Inject]
    public IPostHandler Handler { get; set; }
    [Inject]
    private IDialogService DialogService { get; set; }
    
    

    #endregion
    
    #region Properties
    
    private MudTable<Post> _table;
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

    private async Task<TableData<Post>> ServerReload(TableState state, CancellationToken token)
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
                return new TableData<Post>()
                {
                    TotalItems = result.TotalCount,
                    Items = result.Data ?? new List<Post>()
                };
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<Post> { TotalItems = 0, Items = new List<Post>() };
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
                snackbar.Add("Post data not found for editing.", Severity.Error);
                return; 
            }
        }
        
        var dialog = DialogService.Show<PostForm>("Create/Update Post", parameters);
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
                    snackbar.Add("Post updated successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    snackbar.Add(updateResult.Message, Severity.Error);
                }
            }
            else 
            {
                var newPostRequest = (CreatePostRequest)result.Data;
                var createResult = await Handler.CreateAsync(newPostRequest);

                if (createResult.IsSuccess && createResult.Data is not null)
                {
                    snackbar.Add("Post created successfully!", Severity.Success);
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
            snackbar.Add("Error: Data returned from the form had an incorrect type. Check the form's Update/Create methods.", Severity.Error);
        }
        catch (Exception ex)
        {
            snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
        }
    }

    #endregion
}