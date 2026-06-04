using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using TrashTechHub.Core.Requests;

namespace TrashTechHub.Core.Common.Helpers;

public class ProjectRequestWithImageBase : BaseRequest
{
    [JsonIgnore]
    public IBrowserFile? ImageFile { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}