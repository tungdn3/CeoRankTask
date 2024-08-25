using SeoRankTask.Core.Enums;
using System.Text.RegularExpressions;

namespace CeoRankTask.Core.Services;

public class GoogleExtractor : IExtractor
{
    // Todo: move this to appsettings
    private const string UrlExtractReg = @"<div class=""egMi0 kCrYT""><a\s+href=""\/url\?q=((http:\/\/|https:\/\/)([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+(\.[a-zA-Z]{2,11}))";

    private readonly Regex _extractRegex = new Regex(UrlExtractReg, RegexOptions.IgnoreCase);

    public SearchEngine SearchEngine => SearchEngine.Google;

    public List<string> ExtractUrls(string rawHtml)
    {
        var urls = new List<string>();
        Match match = _extractRegex.Match(rawHtml);
        while (match.Success)
        {
            urls.Add(match.Groups[1].Value);
            match = match.NextMatch();
        }

        return urls;
    }
}
