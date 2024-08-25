using SeoRankTask.Core.Enums;
using System.Text.RegularExpressions;

namespace SeoRankTask.Core.Services;

public class BingExtractor : IExtractor
{
    // Todo: move this to appsettings
    private const string UrlExtractReg = @"<cite>((http:\/\/|https:\/\/)?([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+(\.[a-zA-Z]{2,11}))";

    private readonly Regex _extractRegex = new Regex(UrlExtractReg, RegexOptions.IgnoreCase);

    public SearchEngine SearchEngine => SearchEngine.Bing;

    public List<string> ExtractUrls(string rawHtml)
    {
        var urls = new List<string>();

        // Not very good but it ease the UrlExtractReg
        string withoutStrongTags = Regex.Replace(rawHtml, "(<strong>|</strong>)", string.Empty, RegexOptions.IgnoreCase);
        Match match = _extractRegex.Match(withoutStrongTags);
        while (match.Success)
        {
            urls.Add(match.Groups[1].Value.Replace("<strong>", string.Empty));
            match = match.NextMatch();
        }

        return urls;
    }
}
