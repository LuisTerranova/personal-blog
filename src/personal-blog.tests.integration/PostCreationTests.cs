using System.Net;
using System.Text;
using System.Text.Json;
using personal_blog.core.Models;
using personal_blog.core.Requests.Posts;
using personal_blog.core.Responses;

namespace personal_blog.tests.integration;

public class PostCreationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Xunit.Abstractions.ITestOutputHelper _output;
    
    public PostCreationTests(CustomWebApplicationFactory<Program> factory, Xunit.Abstractions.ITestOutputHelper output)
    {
        _output = output;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRoot_ReturnsOk()
    {
        var response = await _client.GetAsync("/");
        _output.WriteLine($"Root response: {response.StatusCode}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("OK", content);
    }
     
    [Fact]
    public async Task GetAllPosts_ReturnsOk()
    {
        var response = await _client.GetAsync("v1/posts");
        _output.WriteLine($"GetAllPosts response: {response.StatusCode}");
        Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreatePost()
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("TestScheme");
      
        var response = await _client.PostAsJsonAsync("v1/posts", new CreatePostRequest
        {
            Title = "Test title",
            Body = "test body",
            CategoryId = 1
        });
        
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            _output.WriteLine($"CreatePost failed with {response.StatusCode}: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<Response<Post?>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        var newPostId = result.Data.Id;
    }
}