using personal_blog.Api.Common.Api;
using personal_blog.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddSecurity();
builder.AddDataContext();
builder.AddCrossOrigin();
builder.AddDocumentation();
builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseCors("BlazorApp");
app.UseSecurity();
app.MapGet("/", () => new {message = "OK"});
app.MapEndpoints();

app.Run();