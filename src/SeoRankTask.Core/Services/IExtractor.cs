using SeoRankTask.Core.Enums;

namespace SeoRankTask.Core.Services;

public interface IExtractor
{
    SearchEngine SearchEngine { get; }

    /// <summary>
    /// Extract website URLs from a Google search result. The returns URLs are base URL without trailing slash.
    /// </summary>
    /// <param name="rawHtml"></param>
    /// <returns></returns>
    List<string> ExtractUrls(string rawHtml);
}
