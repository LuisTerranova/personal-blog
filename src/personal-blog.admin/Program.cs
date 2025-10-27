using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using personal_blog.admin;
using personal_blog.admin.Handlers;
using personal_blog.admin.Security;
using personal_blog.core.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();
builder.Services.AddTransient<IPostHandler, PostHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IProjectHandler, ProjectHandler>();
builder.Services.AddTransient<IAccountHandler, AccountHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x =>
    (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient("API", (serviceProvider, options) =>
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var apiBaseAddress = configuration.GetValue<string>("ApiBaseAddress");

        if (string.IsNullOrEmpty(apiBaseAddress))
        {
            throw new InvalidOperationException("ApiBaseAddress is not configured. Check wwwroot/appsettings.json.");
        }
        options.BaseAddress = new Uri(apiBaseAddress);
    })
    .AddHttpMessageHandler<CookieHandler>();

await builder.Build().RunAsync();