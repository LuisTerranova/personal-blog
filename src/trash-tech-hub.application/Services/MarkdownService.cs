using Markdig;

using TrashTechHub.Core.Services;

namespace TrashTechHub.Application.Services;

public class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .Build();
    }

    public string ToHtml(string markdown)
    {
        return Markdown.ToHtml(markdown, _pipeline);
    }

    public string ToPlainSummary(string markdown, int maxLength = 200)
    {
        if (string.IsNullOrEmpty(markdown))
            return string.Empty;

        var html = Markdown.ToHtml(markdown, _pipeline);
        var plain = System.Text.RegularExpressions.Regex.Replace(html, "<.*?>", string.Empty).Trim();
        return plain.Length > maxLength
            ? plain[..maxLength] + "..."
            : plain;
    }
}
