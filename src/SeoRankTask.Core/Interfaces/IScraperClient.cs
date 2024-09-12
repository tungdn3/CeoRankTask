using SeoRankTask.Core.Enums;

namespace SeoRankTask.Core.Interfaces;

public interface IScraperClient
{
    SearchEngine SearchEngine { get; }

    Task<string> Scrape(string keyword, int top);
}
