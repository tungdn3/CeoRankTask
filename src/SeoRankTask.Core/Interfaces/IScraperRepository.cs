using SeoRankTask.Core.Enums;

namespace CeoRankTask.Core.Interfaces;

public interface IScraperRepository
{
    SearchEngine SearchEngine { get; }

    Task<string> Scrape(string keyword, int top);
}
