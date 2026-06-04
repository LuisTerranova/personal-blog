namespace TrashTechHub.Core.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName);
    Task DeleteFileAsync(string fileUrl);
}
