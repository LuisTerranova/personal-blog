using Microsoft.AspNetCore.Components;
using MudBlazor;
using personal_blog.core.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Requests.Categories;
using personal_blog.admin.Components.Dashboard.Forms;

namespace personal_blog.admin.Components.Dashboard;

public partial class CategoriesManager
{
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public ICategoryHandler Handler { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    
    #endregion

    #region Properties

    private MudTable<Category> _table = null!;
    private string _searchString = "";
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
    
    private async Task<TableData<Category>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllCategoriesRequest
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize,
                Query = _searchString
            };

            var result = await Handler.GetAllAsync(request);

            if (result.IsSuccess)
            {
                return new TableData<Category>()
                {
                    TotalItems = result.TotalCount,
                    Items = result.Data ?? new List<Category>()
                };
            }
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<Category>() { TotalItems = 0, Items = new List<Category>() };
    }
    
    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();

        if (id.HasValue)
        {
            var getResult = await Handler.GetByIdAsync(new GetCategoryByIdRequest { Id = id.Value });

            if (getResult.IsSuccess && getResult.Data != null)
            {
                var updateModel = new UpdateCategoryRequest
                {
                    Id = getResult.Data.Id,
                    Title = getResult.Data.Title
                };
                
                parameters.Add("InitialUpdateModel", updateModel);
            }
            else
            {
                Snackbar.Add("Category data not found for editing.", Severity.Error);
                return; 
            }
        }
        
        var result = await ( await DialogService.ShowAsync<CategoryForm>("Create/Edit a Category", parameters)).Result;
        
        if (result is { Canceled: true })
            return;
        
        if (result?.Data is null)
        {
            Snackbar.Add("Form submission failed. Data was not returned correctly.", Severity.Error);
            return;
        }

        try
        {
            if (id == null) 
            {
                var newCategoryRequest = (CreateCategoryRequest)result.Data;
                var createResult = await Handler.CreateAsync(newCategoryRequest);

                if (createResult.IsSuccess)
                {
                    Snackbar.Add("Category created successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(createResult.Message ?? "Creation failed with unknown error", Severity.Error);
                }
            }
            else 
            {
                var updateCategoryRequest = (UpdateCategoryRequest)result.Data;
                var updateResult = await Handler.UpdateAsync(updateCategoryRequest);
                
                if (updateResult.IsSuccess)
                {
                    Snackbar.Add(updateResult.Message ?? "Category updated successfully!", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(updateResult.Message ?? "Update failed with unknown error", Severity.Error);
                }
                
            }
        }
        catch(InvalidCastException) 
        {
            Snackbar.Add("Form submission failed. Data was not returned correctly.", Severity.Warning);
        }
        catch(Exception ex)
        {
            Snackbar.Add($"An unexpected error occurred: {ex.Message}", Severity.Error);
        }
    }
    
    private async Task DeleteCategory(int id)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning",
            "Do you really want to delete this category? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeleteCategoryRequest()
                {
                    Id = id
                };
                
                var deleteResult = await Handler.DeleteAsync(request);

                if (deleteResult.IsSuccess)
                {
                    Snackbar.Add(deleteResult.Message ?? "Category deleted successfully", Severity.Success);
                    await _table.ReloadServerData();
                }
                else
                {
                    Snackbar.Add(deleteResult.Message ?? "Deletion failed with unknown error", Severity.Error);
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