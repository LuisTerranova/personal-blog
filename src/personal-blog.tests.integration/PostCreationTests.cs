using System.Net;
using System.Text;
using System.Text.Json;

namespace personal_blog.tests.integration;

public class PostCreationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public PostCreationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreatePost()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme");
        var newPost = new {Title = "Test Title", Body = "Test Body"};
        var content = new StringContent(
            JsonSerializer.Serialize(newPost),
            Encoding.UTF8,
            "application/json");
        var response = await _client.PostAsync("/v1/posts", content);
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var ensureCreation = await _client.GetAsync("/v1/posts/1");
        ensureCreation.EnsureSuccessStatusCode();
    }
}