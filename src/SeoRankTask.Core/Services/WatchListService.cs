using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Entities;
using SeoRankTask.Core.Extensions;
using SeoRankTask.Core.Interfaces;

namespace SeoRankTask.Core.Services;

public class WatchListService : IWatchListService
{
    private const int DefaultPastDays = -15;

    private readonly IWatchListRepository _watchListRepository;

    public WatchListService(IWatchListRepository watchListRepository)
    {
        _watchListRepository = watchListRepository;
    }

    public async Task<PageResultDto<WatchListItemDto>> Get(int pageSize, int pageNumber)
    {
        PageResultDto<WatchListItem> entityResult = await _watchListRepository.Get(pageSize, pageNumber);
        List<WatchListItemDto> dtos = entityResult.Items.Select(x => x.ToDto()).ToList();
        return new PageResultDto<WatchListItemDto>
        {
            Items = dtos,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = entityResult.TotalCount,
        };
    }

    public async Task<IEnumerable<HistoricalRankDto>> GetHistoricalRanks(int id, DateTime? dateFrom)
    {
        DateTime from = dateFrom.HasValue ? dateFrom.Value.Date : DateTime.UtcNow.AddDays(DefaultPastDays).Date;
        List<HistoricalRank> items = await _watchListRepository.GetHistoricalRanks(id, from);
        List<HistoricalRankDto> bestByDays = items
            .GroupBy(x => x.CheckedAt.Date)
            .Select(g =>
            {
                HistoricalRank best = g.OrderBy(x => x.Rank).First();
                return new HistoricalRankDto
                {
                    Id = best.Id,
                    CheckedAt = best.CheckedAt,
                    Rank = best.Rank,
                    WatchListItemId = best.WatchListItemId,
                };
            })
            .ToList();

        return bestByDays;
    }
}
