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
     
        
        var endpointDataSource = _factory.Services.GetRequiredService<EndpointDataSource>();
        var endpoints = endpointDataSource.Endpoints;
        
        var response = await _client.PostAsJsonAsync("v1/posts", new CreatePostRequest
        {
            Title = "Test title",
            Body = "test body"
        });
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<Response<Post?>>();
        var newPostId = result.Data.Id;
        
    }
}