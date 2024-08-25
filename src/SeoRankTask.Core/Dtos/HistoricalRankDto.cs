namespace SeoRankTask.Core.Dtos;

public class HistoricalRankDto
{
    public int Id { get; set; }

    public int WatchListItemId { get; set; }

    public int Rank { get; set; }

    public DateTime CheckedAt { get; set; }
}
