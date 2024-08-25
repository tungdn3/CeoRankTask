namespace SeoRankTask.Core.Entities;

public class HistoricalRank
{
    public int Id { get; set; }

    public int WatchListItemId { get; set; }

    public int Rank { get; set; }

    public DateTime CheckedAt { get; set; }

    public WatchListItem WatchListItem { get; set; }
}
