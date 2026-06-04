using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TrashTechHub.Infrastructure.Data;
using TrashTechHub.Infrastructure.Repositories;
using TrashTechHub.Application.Services;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Requests.Posts;
using TrashTechHub.Core.Requests.Projects;
using TrashTechHub.Core.Requests.Categories;
using TrashTechHub.Core.DTOs;
using Microsoft.Extensions.Logging;
using AutoMapper;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Tests;

public class PostTests
{
    private readonly AppDbContext _context;
    private readonly IPostService _service;
    private readonly IMapper _mapper;
    private readonly UnitOfWork _unitOfWork;

    public PostTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);

        _context.Categories.Add(new Category { Id = 1, Title = "Tech", Slug = "tech" });
        _context.Categories.Add(new Category { Id = 2, Title = "General", Slug = "general" });
        _context.SaveChanges();

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<TrashTechHub.Application.Mappings.MappingProfile>();
        }, NullLoggerFactory.Instance);
        _mapper = config.CreateMapper();
        _unitOfWork = new UnitOfWork(_context);

        var repository = new PostRepository(_context, NullLogger<PostRepository>.Instance);
        var markdown = new MarkdownService();
        var logger = NullLogger<PostService>.Instance;
        _service = new PostService(repository, _unitOfWork, _mapper, markdown, logger);
    }

    [Fact]
    public async Task CreatePost_ValidRequest_ReturnsPost()
    {
        var request = new CreatePostRequest
        {
            Title = "Test Post Title",
            Body = "# Test Body\nThis is a test post.",
            CategoryId = 1
        };

        var post = await _service.CreateAsync(request);

        Assert.NotNull(post);
        Assert.Equal("Test Post Title", post.Title);
        Assert.Equal("# Test Body\nThis is a test post.", post.Body);
    }

    [Fact]
    public async Task GetPostById_ExistingPost_ReturnsPost()
    {
        var post = new Post
        {
            Title = "Post To Get",
            Body = "Body content",
            CategoryId = 2
        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        var result = await _service.GetByIdAsync(new GetPostByIdRequest { Id = post.Id });

        Assert.NotNull(result);
        Assert.Equal("Post To Get", result.Title);
    }

    [Fact]
    public async Task DeletePost_ExistingPost_RemovesPost()
    {
        var post = new Post
        {
            Title = "Post To Delete",
            Body = "Body content",
            CategoryId = 1
        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        var deleted = await _service.DeleteAsync(new DeletePostRequest { Id = post.Id });

        Assert.True(deleted);

        var checkDb = await _context.Posts.FindAsync(post.Id);
        Assert.Null(checkDb);
    }

    [Fact]
    public async Task UpdatePost_ValidRequest_UpdatesPost()
    {
        var post = new Post
        {
            Title = "Original Title",
            Body = "Original Body",
            CategoryId = 1
        };
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();

        // Detach the entity to simulate detached behavior
        _context.Entry(post).State = EntityState.Detached;

        var request = new UpdatePostRequest
        {
            Id = post.Id,
            Title = "Updated Title",
            Body = "Updated Body",
            CategoryId = 2,
            IsFeatured = true
        };

        var result = await _service.UpdateAsync(request);

        Assert.NotNull(result);
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Body", result.Body);
        Assert.Equal(2, result.CategoryId);
        Assert.True(result.IsFeatured);

        // Verify DB updates
        var checkDb = await _context.Posts.AsNoTracking().Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == post.Id);
        Assert.NotNull(checkDb);
        Assert.Equal("Updated Title", checkDb.Title);
        Assert.Equal("Updated Body", checkDb.Body);
        Assert.Equal(2, checkDb.CategoryId);
        Assert.Equal("General", checkDb.Category?.Title);
    }

    [Fact]
    public async Task UpdateCategory_DuplicateSlug_ThrowsException()
    {
        var categoryRepo = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);
        var categoryService = new CategoryService(categoryRepo, _unitOfWork, _mapper, NullLogger<CategoryService>.Instance);

        // Category 1 is "Tech" (slug: "tech"), Category 2 is "General" (slug: "general")
        var updateRequest = new TrashTechHub.Core.Requests.Categories.UpdateCategoryRequest
        {
            Id = 2,
            Title = "Tech" // slug: "tech" which already exists for category 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.UpdateAsync(updateRequest));
    }

    [Fact]
    public async Task UpdateCategory_ValidRequest_UpdatesCategory()
    {
        var categoryRepo = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);
        var categoryService = new CategoryService(categoryRepo, _unitOfWork, _mapper, NullLogger<CategoryService>.Instance);

        var updateRequest = new TrashTechHub.Core.Requests.Categories.UpdateCategoryRequest
        {
            Id = 2,
            Title = "New General Title"
        };

        var updated = await categoryService.UpdateAsync(updateRequest);

        Assert.NotNull(updated);
        Assert.Equal("New General Title", updated.Title);
        Assert.Equal("new-general-title", updated.Slug);
    }

    [Fact]
    public async Task GetPostById_NonExistentId_ReturnsNull()
    {
        var result = await _service.GetByIdAsync(new GetPostByIdRequest { Id = 999 });

        Assert.Null(result);
    }

    [Fact]
    public async Task DeletePost_NonExistentId_ReturnsFalse()
    {
        var result = await _service.DeleteAsync(new DeletePostRequest { Id = 999 });

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllPosts_Pagination_ReturnsCorrectPage()
    {
        for (var i = 1; i <= 12; i++)
        {
            await _context.Posts.AddAsync(new Post
            {
                Title = $"Post {i}",
                Body = "Body",
                CategoryId = 1,
                Updated = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        var result = await _service.GetAllAsync(new GetAllPostsRequest
        {
            PageNumber = 2,
            PageSize = 5
        });

        Assert.Equal(12, result.TotalCount);
        Assert.Equal(5, result.Items.Count);
    }

    [Fact]
    public async Task CreatePost_PersistsToDatabase()
    {
        var request = new CreatePostRequest
        {
            Title = "Persisted Post",
            Body = "Body content",
            CategoryId = 1
        };

        var post = await _service.CreateAsync(request);

        var dbPost = await _context.Posts.FindAsync(post.Id);
        Assert.NotNull(dbPost);
        Assert.Equal("Persisted Post", dbPost.Title);
        Assert.Equal("Body content", dbPost.Body);
    }

    [Fact]
    public async Task GetFeaturedPosts_NoFeaturedPosts_FallsBackToRecent()
    {
        for (var i = 1; i <= 3; i++)
        {
            await _context.Posts.AddAsync(new Post
            {
                Title = $"Recent Post {i}",
                Body = "Body",
                CategoryId = 1,
                Updated = DateTime.UtcNow
            });
        }
        await _context.SaveChangesAsync();

        var result = await _service.GetFeaturedAsync(new GetFeaturedPostsRequest { PageSize = 5 });

        Assert.NotEmpty(result.Items);
        Assert.Equal(3, result.TotalCount);
    }

    [Fact]
    public async Task CreateProject_ValidRequest_ReturnsProject()
    {
        var repository = new ProjectRepository(_context, NullLogger<ProjectRepository>.Instance);
        var projectService = new ProjectService(repository, _unitOfWork, _mapper, new DummyFileStorageService(), NullLogger<ProjectService>.Instance);

        var request = new CreateProjectRequest
        {
            Title = "Test Project",
            Summary = "Test Summary",
            Description = "Test Description",
            RepoLink = "https://github.com/test"
        };

        var project = await projectService.CreateAsync(request);

        Assert.NotNull(project);
        Assert.Equal("Test Project", project.Title);
        Assert.Equal("Test Summary", project.Summary);
    }

    [Fact]
    public async Task GetProjectById_ExistingProject_ReturnsProject()
    {
        var repository = new ProjectRepository(_context, NullLogger<ProjectRepository>.Instance);
        var projectService = new ProjectService(repository, _unitOfWork, _mapper, new DummyFileStorageService(), NullLogger<ProjectService>.Instance);

        var created = await projectService.CreateAsync(new CreateProjectRequest
        {
            Title = "Find Me",
            Summary = "Summary",
            Description = "Description"
        });

        var found = await projectService.GetByIdAsync(new GetProjectByIdRequest { Id = created.Id });

        Assert.NotNull(found);
        Assert.Equal("Find Me", found.Title);
    }

    [Fact]
    public async Task DeleteProject_ExistingProject_RemovesProject()
    {
        var repository = new ProjectRepository(_context, NullLogger<ProjectRepository>.Instance);
        var projectService = new ProjectService(repository, _unitOfWork, _mapper, new DummyFileStorageService(), NullLogger<ProjectService>.Instance);

        var created = await projectService.CreateAsync(new CreateProjectRequest
        {
            Title = "To Delete",
            Summary = "Summary",
            Description = "Description"
        });

        var deleted = await projectService.DeleteAsync(new DeleteProjectRequest { Id = created.Id });
        Assert.True(deleted);

        var found = await projectService.GetByIdAsync(new GetProjectByIdRequest { Id = created.Id });
        Assert.Null(found);
    }

    [Fact]
    public async Task UpdateProject_ValidRequest_UpdatesProject()
    {
        var repository = new ProjectRepository(_context, NullLogger<ProjectRepository>.Instance);
        var projectService = new ProjectService(repository, _unitOfWork, _mapper, new DummyFileStorageService(), NullLogger<ProjectService>.Instance);

        var created = await projectService.CreateAsync(new CreateProjectRequest
        {
            Title = "Original Title",
            Summary = "Original Summary",
            Description = "Original Description"
        });

        var updateRequest = new UpdateProjectRequest
        {
            Id = created.Id,
            Title = "Updated Title",
            Summary = "Updated Summary",
            Description = "Updated Description"
        };

        var updated = await projectService.UpdateAsync(updateRequest);

        Assert.NotNull(updated);
        Assert.Equal("Updated Title", updated.Title);
        Assert.Equal("Updated Summary", updated.Summary);
    }

    [Fact]
    public async Task GetProjectById_NonExistent_ReturnsNull()
    {
        var repository = new ProjectRepository(_context, NullLogger<ProjectRepository>.Instance);
        var projectService = new ProjectService(repository, _unitOfWork, _mapper, new DummyFileStorageService(), NullLogger<ProjectService>.Instance);

        var found = await projectService.GetByIdAsync(new GetProjectByIdRequest { Id = 9999 });
        Assert.Null(found);
    }

    [Fact]
    public async Task CreateCategory_DuplicateSlug_ThrowsException()
    {
        var repository = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);
        var categoryService = new CategoryService(repository, _unitOfWork, _mapper, NullLogger<CategoryService>.Instance);

        var request1 = new CreateCategoryRequest { Title = "Duplicate Title" };
        await categoryService.CreateAsync(request1);

        var request2 = new CreateCategoryRequest { Title = "Duplicate Title" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => categoryService.CreateAsync(request2));
    }

    [Fact]
    public async Task GetAllPosts_SearchFilter_ReturnsFilteredResults()
    {
        await _context.Posts.AddAsync(new Post
        {
            Title = "Unique Apple Post",
            Body = "Body content",
            CategoryId = 1,
            Updated = DateTime.UtcNow
        });
        await _context.Posts.AddAsync(new Post
        {
            Title = "Boring Orange Post",
            Body = "Body content",
            CategoryId = 1,
            Updated = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();

        var result = await _service.GetAllAsync(new GetAllPostsRequest { Query = "Apple" });

        Assert.Single(result.Items);
        Assert.Equal("Unique Apple Post", result.Items[0].Title);
    }
}

public class DummyFileStorageService : IFileStorageService
{
    public Task<string> SaveFileAsync(Stream fileStream, string fileName, string folderName) => Task.FromResult("/dummy/url");
    public Task DeleteFileAsync(string fileUrl) => Task.CompletedTask;
}
