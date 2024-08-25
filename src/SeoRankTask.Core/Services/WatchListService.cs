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
        // Todo: group by date -> select max
        return items.Select(x => x.ToDto());
    }
}
