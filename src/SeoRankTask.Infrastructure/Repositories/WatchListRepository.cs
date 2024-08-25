using Microsoft.EntityFrameworkCore;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Entities;
using SeoRankTask.Core.Interfaces;

namespace SeoRankTask.Infrastructure.Repositories;

public class WatchListRepository : IWatchListRepository
{
    private readonly SeoRankContext _context;

    public WatchListRepository(SeoRankContext context)
    {
        _context = context;
    }

    public async Task<PageResultDto<WatchListItem>> Get(int pageSize, int pageNumber)
    {
        var query = _context.WatchListItems.AsNoTracking();

        int totalCount = await query.CountAsync();

        List<WatchListItem> items = await query.ToListAsync();

        return new PageResultDto<WatchListItem>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public Task<List<HistoricalRank>> GetHistoricalRanks(int watchListItemId, DateTime from)
    {
        return _context.HistoricalRanks
            .AsNoTracking()
            .Where(x => x.WatchListItemId == watchListItemId && x.CheckedAt >= from)
            
            // To support the demo, I have seeded data for the next month. Need to filter out.
            .Where(x => x.CheckedAt < DateTime.UtcNow)
            .ToListAsync();
    }
}
