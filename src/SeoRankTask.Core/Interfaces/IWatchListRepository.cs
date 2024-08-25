using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Entities;

namespace SeoRankTask.Core.Interfaces;

public interface IWatchListRepository
{
    Task<PageResultDto<WatchListItem>> Get(int pageSize, int pageNumber);

    Task<List<WatchListItem>> GetAll();

    Task<List<HistoricalRank>> GetHistoricalRanks(int watchListItemId, DateTime from);
    
    Task SaveHistoricalRank(HistoricalRank historicalRank);
}
