using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrashTechHub.Infrastructure.Data;
using TrashTechHub.Infrastructure.Repositories;
using TrashTechHub.Infrastructure.Services;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(x =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                x.UseNpgsql(connectionString);
            }
        });

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
