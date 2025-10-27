using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using personal_blog.core.Handlers.FrontEndHandlers;
using personal_blog.front;
using personal_blog.front.Handlers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IFrontPostHandler, PostHandler>();
builder.Services.AddTransient<IFrontCategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IFrontProjectHandler, ProjectHandler>();

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
});

await builder.Build().RunAsync();