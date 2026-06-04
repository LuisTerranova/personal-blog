using Microsoft.AspNetCore.Hosting;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Infrastructure.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    private static readonly HashSet<string> _allowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName)
    {
        var fileExtension = Path.GetExtension(fileName);
        if (!_allowedExtensions.Contains(fileExtension))
            throw new InvalidOperationException($"File extension '{fileExtension}' is not allowed. Allowed: {string.Join(", ", _allowedExtensions)}");

        var basePath = Path.Combine(env.WebRootPath, folderName);

        if (!Directory.Exists(basePath))
            Directory.CreateDirectory(basePath);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var fullPath = Path.Combine(basePath, uniqueFileName);

        await using var diskStream = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(diskStream);

        return $"/{folderName}/{uniqueFileName}";
    }

    public Task DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return Task.CompletedTask;

        var wwwroot = env.WebRootPath;
        var relativePath = fileUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.GetFullPath(Path.Combine(wwwroot, relativePath));

        if (!fullPath.StartsWith(wwwroot, StringComparison.OrdinalIgnoreCase))
            return Task.CompletedTask;

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
