using System.ComponentModel.DataAnnotations;

namespace TrashTechHub.Core.Requests.Posts;

public class GetPostByIdRequest : BaseRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Invalid post Id")]
    public int Id { get; set; }
}