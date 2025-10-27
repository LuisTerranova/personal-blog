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

builder.Services.AddHttpClient("API", options =>
{
    options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ApiBaseAddress"));
});

await builder.Build().RunAsync();