namespace CeoRankTask.Core.Services;

public interface IGoogleExtractor
{
    /// <summary>
    /// Extract website URLs from a Google search result. The returns URLs are base URL without trailing slash.
    /// </summary>
    /// <param name="rawHtml"></param>
    /// <returns></returns>
    List<string> ExtractUrls(string rawHtml);
}
