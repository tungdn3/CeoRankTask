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

        var ran = new Random();
        var now = DateTime.UtcNow;

        var watchListItem1 = new WatchListItem { Keyword = "land registry search", Url = "www.infotrack.co.uk" };
        context.WatchListItems.Add(watchListItem1);
        context.SaveChanges();

        for (int i = -30; i <= 30; i++)
        {
            var historicalRank = new HistoricalRank
            {
                WatchListItemId = watchListItem1.Id,
                Rank = ran.Next(10, 30),
                CheckedAt = now.AddDays(i),
            };
            context.HistoricalRanks.Add(historicalRank);
        }
        context.SaveChanges();

        var watchListItem2 = new WatchListItem { Keyword = "Digital Conveyancing", Url = "www.infotrack.co.uk" };
        context.WatchListItems.Add(watchListItem2);
        context.SaveChanges();

        for (int i = -30; i <= 30; i++)
        {
            var historicalRank = new HistoricalRank
            {
                WatchListItemId = watchListItem2.Id,
                Rank = ran.Next(10, 50),
                CheckedAt = now.AddDays(i),
            };
            context.HistoricalRanks.Add(historicalRank);
        }
        context.SaveChanges();
    }
}
