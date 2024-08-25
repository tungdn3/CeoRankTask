using SeoRankTask.Core.Enums;

namespace SeoRankTask.Core.Interfaces;

public interface IScraperRepository
{
    SearchEngine SearchEngine { get; }

    Task<string> Scrape(string keyword, int top);
}
