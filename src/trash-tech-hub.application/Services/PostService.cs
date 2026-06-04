using Microsoft.Extensions.Logging;
using AutoMapper;
using TrashTechHub.Core.Common;
using TrashTechHub.Core.DTOs;
using TrashTechHub.Core.Models;
using TrashTechHub.Core.Repositories;
using TrashTechHub.Core.Requests.Posts;
using TrashTechHub.Core.Services;

namespace TrashTechHub.Application.Services;

public class PostService(
    IPostRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IMarkdownService markdownService,
    ILogger<PostService> logger) : IPostService
{
    public async Task<PagedResult<PostDto>> GetAllAsync(GetAllPostsRequest request)
    {
        var (posts, totalCount) = await repository.GetAllAsync(request);
        var dtos = mapper.Map<List<PostDto>>(posts);
        foreach (var dto in dtos)
        {
            dto.Body = markdownService.ToPlainSummary(dto.Body);
        }
        return new PagedResult<PostDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }

    public async Task<PostDto?> GetByIdAsync(GetPostByIdRequest request)
    {
        var post = await repository.GetByIdAsync(request.Id);
        return mapper.Map<PostDto>(post);
    }

    public async Task<PostDto?> GetBySlugAsync(string slug)
    {
        var post = await repository.GetBySlugAsync(slug);
        return mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> CreateAsync(CreatePostRequest request)
    {
        var post = new Post
        {
            Title = request.Title,
            Body = request.Body,
            CategoryId = request.CategoryId,
            IsFeatured = request.IsFeatured,
            Slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : request.Slug,
            Excerpt = GenerateExcerpt(request.Body, request.Excerpt)
        };

        await repository.CreateAsync(post);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Created post: {Id} ({Title})", post.Id, post.Title);

        return mapper.Map<PostDto>(post);
    }

    public async Task<PostDto?> UpdateAsync(UpdatePostRequest request)
    {
        var existing = await repository.GetByIdAsync(request.Id);
        if (existing == null) return null;

        existing.Title = request.Title;
        existing.Body = request.Body;
        existing.CategoryId = request.CategoryId;
        existing.IsFeatured = request.IsFeatured;
        existing.Slug = string.IsNullOrWhiteSpace(request.Slug) ? GenerateSlug(request.Title) : request.Slug;
        existing.Excerpt = GenerateExcerpt(request.Body, request.Excerpt);
        existing.Updated = DateTime.UtcNow;

        await repository.UpdateAsync(existing);
        await unitOfWork.SaveChangesAsync();
        logger.LogInformation("Updated post: {Id}", existing.Id);

        return mapper.Map<PostDto>(existing);
    }

    private string GenerateSlug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Guid.NewGuid().ToString("n")[..8];
        var slug = title.ToLowerInvariant();
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
        slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }

    private string GenerateExcerpt(string body, string? requestExcerpt)
    {
        if (!string.IsNullOrWhiteSpace(requestExcerpt))
            return requestExcerpt.Length > 300 ? requestExcerpt[..297] + "..." : requestExcerpt;
        return markdownService.ToPlainSummary(body, 250);
    }

    public async Task<bool> DeleteAsync(DeletePostRequest request)
    {
        var result = await repository.DeleteAsync(request.Id);
        if (result)
        {
            await unitOfWork.SaveChangesAsync();
            logger.LogInformation("Deleted post: {Id}", request.Id);
        }
        return result;
    }

    public async Task<PagedResult<PostDto>> GetFeaturedAsync(GetFeaturedPostsRequest request)
    {
        var (posts, totalCount) = await repository.GetFeaturedAsync(request.PageSize);
        var dtos = mapper.Map<List<PostDto>>(posts);
        foreach (var dto in dtos)
        {
            dto.Body = markdownService.ToPlainSummary(dto.Body);
        }
        return new PagedResult<PostDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
