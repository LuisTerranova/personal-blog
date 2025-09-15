using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddHttpClient("Api", client => 
    client.BaseAddress = new Uri("http://localhost:5177"));

await builder.Build().RunAsync();
