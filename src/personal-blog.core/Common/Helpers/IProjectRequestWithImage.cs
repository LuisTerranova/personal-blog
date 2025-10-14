using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using personal_blog.core.Requests;

namespace personal_blog.core.Common.Helpers;

public class IProjectRequestWithImage : BaseRequest
{
    [JsonIgnore]
    public IBrowserFile? ImageFile { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}