namespace SeoRankTask.Core.Entities;

public class WatchListItem
{
    public int Id { get; set; }

    public string Keyword { get; set; } = string.Empty;

    public string Url { get; set; } = string.Empty;

    public ICollection<HistoricalRank> HistoricalRanks { get; set; } = new List<HistoricalRank>();
}
