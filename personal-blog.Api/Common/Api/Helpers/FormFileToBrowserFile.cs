using Microsoft.AspNetCore.Components.Forms;

namespace personal_blog.Api.Common.Api.Helpers;

public class FormFileToBrowserFile : IBrowserFile
{
    private readonly IFormFile _file;

    public FormFileToBrowserFile(IFormFile file)
    {
        _file = file;
    }

    public Stream OpenReadStream(long maxAllowedSize = 52428800, CancellationToken cancellationToken = new CancellationToken())
    {
        return _file.OpenReadStream();
    }

    public string Name => _file.FileName;
    public long Size => _file.Length;
    public string ContentType => _file.ContentType;
    public DateTimeOffset LastModified => DateTimeOffset.Now;
    
}