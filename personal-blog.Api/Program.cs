using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.ApiTesting;
using personal_blog.Api.Data;
using personal_blog.Api.Endpoints;
using personal_blog.Api.Handlers;
using personal_blog.core.Models;
using personal_blog.core.Handlers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder
    .Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<IPostHandler, PostHandler>();
builder.Services.AddTransient<IProjectHandler, ProjectHandler>();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(x => { x.UseSqlServer(connectionString); });

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole<long>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();


builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorApp", policy =>
    {
        policy.WithOrigins("http://localhost:5096", "https://localhost:5096")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
});

var app = builder.Build();

await app.SeedAdminUserAsync(); //Seeding the database for testing

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("BlazorApp");
app.MapGet("/", () => new {message = "OK"});
app.MapEndpoints();

app.Run();
