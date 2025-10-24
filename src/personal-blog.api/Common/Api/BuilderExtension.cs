using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using personal_blog.Api.Data;
using personal_blog.Api.Handlers;
using personal_blog.Api.Models;
using personal_blog.core;
using personal_blog.core.Handlers;

namespace personal_blog.Api.Common.Api;

public static class BuilderExtension
{
    public static void AddConfiguration(this WebApplicationBuilder builder)
    {
        Configuration.ConnectionString =
            builder
                .Configuration
                .GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public static void AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.CustomSchemaIds(n => n.FullName);
        });
    }

    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
            .AddIdentityCookies();
        builder.Services.AddAuthorization();
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
        builder.Services.AddTransient<IPostHandler, PostHandler>();
        builder.Services.AddTransient<IProjectHandler, ProjectHandler>();
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("BlazorApp", policy =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    policy.WithOrigins(
                            "http://localhost:5096",
                            "https://localhost:5096",
                            "http://localhost:5164",
                            "https://localhost:5164",
                            //Docker
                            "http://localhost:8080",  
                            "http://localhost:8082"   
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
                else
                {
                    var origins = builder.Configuration.GetValue<string>("CORS_ORIGINS");
                    if (!string.IsNullOrEmpty(origins))
                    {
                        policy.WithOrigins(origins.Split(','))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials(); 
                    }
                }
            });
        });
    }


    public static void AddDataContext(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDbContext<AppDbContext>
                (x => { x.UseNpgsql(Configuration.ConnectionString); });   
        builder.Services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<long>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();
    }
}