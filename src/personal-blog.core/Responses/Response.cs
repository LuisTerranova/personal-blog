using System.Text.Json.Serialization;

namespace personal_blog.core.Responses;

public class Response<TData>
{
    public Response() => _code = Configuration.DefaultStatusCode;
    
    private readonly int _code;
    
    public Response(TData data, string? message = null, int code = Configuration.DefaultStatusCode)
    {
        Data = data;
        Message = message;
        _code = code;
    }
    
    public TData? Data { get; set; }
    public string? Message { get; set; } = string.Empty;
    
    [JsonIgnore]
    public bool IsSuccess
       => _code is >= 200 and <= 299;
    
    public int StatusCode => _code;
}