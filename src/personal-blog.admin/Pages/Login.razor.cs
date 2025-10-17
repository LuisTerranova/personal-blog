using Microsoft.AspNetCore.Components;
using MudBlazor;
using personal_blog.admin.Security;
using personal_blog.core.Handlers;
using personal_blog.core.Requests;

namespace personal_blog.admin.Pages;

public class Login_ : ComponentBase
{
    #region Services
    
    [Inject]
    public ISnackbar? Snackbar { get; set; }
    
    [Inject]
    public IAccountHandler? AccountHandler { get; set; }
    
    [Inject]
    public NavigationManager? NavigationManager { get; set; }
    
    [Inject]
    public ICookieAuthenticationStateProvider? AuthenticationStateProvider { get; set; }
    
    #endregion
    
    #region Properties
    
    public bool IsBusy { get; set; }
    public readonly LoginRequest InputModel = new();
    
    #endregion
    
    #region Overrides

    protected override async Task OnInitializedAsync()
    {
        var authState =  await AuthenticationStateProvider!.GetAuthenticationStateAsync();
        var user = authState.User;
        
        if (user.Identity is {IsAuthenticated: true})
            NavigationManager!.NavigateTo("/dashboard");
    }
    
    #endregion

    #region Methods

    public async Task HandleLogin()
    {
        IsBusy = true;

        try
        {
            var result = await AccountHandler.LoginAsync(InputModel);

            if (result.IsSuccess)
            {
                await AuthenticationStateProvider!.GetAuthenticationStateAsync();
                AuthenticationStateProvider.NotifyAuthenticationStateChanged();
                NavigationManager!.NavigateTo("/dashboard");
            }
            else
                Snackbar!.Add(result.Message ?? string.Empty, Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar!.Add(ex.Message, Severity.Error);
        }
        finally
        {
            IsBusy = false;
        }
    }

    #endregion
}