using TrashTechHub.Core.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Categories;
using TrashTechHub.Core.Services;
using TrashTechHub.Web.Components.Admin.Dashboard.Forms;

namespace TrashTechHub.Web.Components.Admin.Dashboard;

public partial class CategoriesManager
{
    #region Services

    [Inject]
    public ISnackbar Snackbar { get; set; } = null!;
    [Inject]
    public ICategoryService Service { get; set; } = null!;
    [Inject]
    private IDialogService DialogService { get; set; } = null!;
    
    #endregion

    #region Properties

    private MudTable<CategoryDto> _table = null!;
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
    
    private async Task<TableData<CategoryDto>> ServerReload(TableState state, CancellationToken token)
    {
        try
        {
            var request = new GetAllCategoriesRequest
            {
                PageNumber = state.Page + 1, 
                PageSize = state.PageSize,
                Query = _searchString
            };

            var result = await Service.GetAllAsync(request);
            return new TableData<CategoryDto>() { TotalItems = result.TotalCount, Items = result.Items };
        }
        catch (Exception ex)
        {
            _errorMessage = $"An unexpected error occurred: {ex.Message}";
            Snackbar.Add(_errorMessage, Severity.Error);
        }
        return new TableData<CategoryDto>() { TotalItems = 0, Items = new List<CategoryDto>() };
    }
    
    private async Task OpenForm(int? id = null)
    {
        var parameters = new DialogParameters();

        if (id.HasValue)
        {
            var category = await Service.GetByIdAsync(new GetCategoryByIdRequest { Id = id.Value });

            if (category != null)
            {
                var updateModel = new UpdateCategoryRequest
                {
                    Id = category.Id,
                    Title = category.Title
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
                await Service.CreateAsync(newCategoryRequest);
                Snackbar.Add("Category created successfully!", Severity.Success);
                await _table.ReloadServerData();
            }
            else 
            {
                var updateCategoryRequest = (UpdateCategoryRequest)result.Data;
                await Service.UpdateAsync(updateCategoryRequest);
                Snackbar.Add("Category updated successfully!", Severity.Success);
                await _table.ReloadServerData();
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
        bool? result = await DialogService.ShowMessageBoxAsync(
            "Warning",
            "Do you really want to delete this category? This action cannot be undone.",
            yesText: "Delete!",
            cancelText: "Cancel");

        if (result is true)
        {
            try
            {
                var request = new DeleteCategoryRequest() { Id = id };
                var deleted = await Service.DeleteAsync(request);
                if (deleted)
                {
                    Snackbar.Add("Category deleted successfully", Severity.Success);
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