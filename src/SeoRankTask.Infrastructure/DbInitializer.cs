using SeoRankTask.Core.Entities;

namespace SeoRankTask.Infrastructure;

public static class DbInitializer
{
    public static void Initialize(SeoRankContext context)
    {
        context.Database.EnsureCreated();

        if (context.WatchListItems.Any())
        {
            return; // DB has been seeded
        }

        var watchListItem = new WatchListItem { Keyword = "land registry search", Url = "www.infotrack.co.uk" };
        context.WatchListItems.Add(watchListItem);
        context.SaveChanges();

        var ran = new Random();
        var now = DateTime.UtcNow;
        for (int i = -30; i <= 30; i++)
        {
            var historicalRank = new HistoricalRank
            {
                WatchListItemId = watchListItem.Id,
                Rank = ran.Next(10, 50),
                CheckedAt = now.AddDays(i),
            };
            context.HistoricalRanks.Add(historicalRank);
        }
        context.SaveChanges();
    }
}
