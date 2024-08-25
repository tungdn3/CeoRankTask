using SeoRankTask.Core.Dtos;

namespace SeoRankTask.Core.Services;

public interface IWatchListService
{
    Task<PageResultDto<WatchListItemDto>> Get(int pageSize, int pageNumber);
    
    Task<IEnumerable<HistoricalRankDto>> GetHistoricalRanks(int id, DateTime? dateFrom);
}
