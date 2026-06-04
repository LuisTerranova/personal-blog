namespace TrashTechHub.Core.Requests.Projects;

public class GetAllProjectsRequest : PagedRequest
{
    public string? Query { get; set; }
}