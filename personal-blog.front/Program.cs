using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using personal_blog.core.Handlers;
using personal_blog.front;
using personal_blog.front.Handlers;
using personal_blog.front.Security;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<CookieHandler>();
builder.Services.AddTransient<IPostHandler, PostHandler>();
builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IProjectHandler, ProjectHandler>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddScoped(x =>
    (ICookieAuthenticationStateProvider)x.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddMudServices();

builder.Services.AddHttpClient("API", options =>
{
    options.BaseAddress = new Uri("http://localhost:5177");
})
.AddHttpMessageHandler<CookieHandler>();


builder.Services.AddTransient<IAccountHandler, AccountHandler>();

await builder.Build().RunAsync();