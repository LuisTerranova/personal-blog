namespace TrashTechHub.Core.Services;

public interface IMarkdownService
{
    string ToHtml(string markdown);
    string ToPlainSummary(string markdown, int maxLength = 200);
}
