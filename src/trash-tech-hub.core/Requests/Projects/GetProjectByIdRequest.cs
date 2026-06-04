using System.ComponentModel.DataAnnotations;

namespace TrashTechHub.Core.Requests.Projects;

public class GetProjectByIdRequest : BaseRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid project Id")]
    public int Id { get; set; }
}