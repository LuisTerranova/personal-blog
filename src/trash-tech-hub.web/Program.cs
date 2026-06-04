using DotNetEnv;
using Microsoft.AspNetCore.Authentication;
using MudBlazor.Services;
using TrashTechHub.Web.Common.Startup;
using TrashTechHub.Web.Components;
using TrashTechHub.Core.Requests.Posts;
using TrashTechHub.Core.Requests.Projects;
using TrashTechHub.Core.Services;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(builder.Environment.ContentRootPath, "..", ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    builder.Configuration["AdminSettings:Email"] = Env.GetString("ADMIN_EMAIL");
    builder.Configuration["AdminSettings:Password"] = Env.GetString("ADMIN_PASSWORD");
}

builder.AddSecurity();
builder.AddDataContext();
builder.AddServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMudServices();

var app = builder.Build();

await app.ConfigureEnvironment();
app.UseStaticFiles();
app.UseSecurity();
app.UseAntiforgery();
app.MapGet("/health", () => new {message = "OK"});

app.MapGet("/sitemap.xml", async (
    IPostService postService, 
    IProjectService projectService, 
    HttpContext context) =>
{
    var domain = "https://trashtechhub.com";
    var xml = new System.Text.StringBuilder();
    xml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    xml.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

    // Add static pages
    AddUrl(xml, $"{domain}/", DateTime.UtcNow, "daily", 1.0);
    AddUrl(xml, $"{domain}/blog", DateTime.UtcNow, "daily", 0.8);
    AddUrl(xml, $"{domain}/projects", DateTime.UtcNow, "weekly", 0.8);
    AddUrl(xml, $"{domain}/about", DateTime.UtcNow, "monthly", 0.7);

    // Fetch blog posts
    try
    {
        var postsResult = await postService.GetAllAsync(new GetAllPostsRequest { PageNumber = 1, PageSize = 1000 });
        foreach (var post in postsResult.Items)
        {
            var date = post.Updated ?? post.Created;
            AddUrl(xml, $"{domain}/blog/{post.Slug}", date, "weekly", 0.6);
        }
    }
    catch { /* ignore */ }

    // Fetch projects
    try
    {
        var projectsResult = await projectService.GetAllAsync(new GetAllProjectsRequest { PageNumber = 1, PageSize = 1000 });
        foreach (var project in projectsResult.Items)
        {
            AddUrl(xml, $"{domain}/projects/{project.Slug}", project.Created, "monthly", 0.5);
        }
    }
    catch { /* ignore */ }

    xml.AppendLine("</urlset>");
    
    context.Response.ContentType = "application/xml";
    await context.Response.WriteAsync(xml.ToString());

    static void AddUrl(System.Text.StringBuilder xmlBuilder, string url, DateTime lastModified, string changeFrequency, double priority)
    {
        xmlBuilder.AppendLine("  <url>");
        xmlBuilder.AppendLine($"    <loc>{url}</loc>");
        xmlBuilder.AppendLine($"    <lastmod>{lastModified:yyyy-MM-dd}</lastmod>");
        xmlBuilder.AppendLine($"    <changefreq>{changeFrequency}</changefreq>");
        xmlBuilder.AppendLine($"    <priority>{priority:0.0}</priority>");
        xmlBuilder.AppendLine("  </url>");
    }
});

app.MapGet("/blog/{Id:int}", async (int Id, IPostService postService) =>
{
    var post = await postService.GetByIdAsync(new GetPostByIdRequest { Id = Id });
    if (post == null)
    {
        return Results.NotFound();
    }
    return Results.Redirect($"/blog/{post.Slug}", permanent: true);
});

app.MapPost("/logout", async (HttpContext httpContext) =>
{
    await httpContext.SignOutAsync("Cookies");
    return Results.Redirect("/");
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
public partial class Program { }
