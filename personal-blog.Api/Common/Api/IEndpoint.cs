namespace personal_blog.Api.Common.Api;

public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder endpoints);
}