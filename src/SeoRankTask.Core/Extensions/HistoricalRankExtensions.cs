using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Entities;

namespace SeoRankTask.Core.Extensions;

public static class HistoricalRankExtensions
{
    public static HistoricalRankDto ToDto(this HistoricalRank entity)
    {
        return new HistoricalRankDto
        {
            Id = entity.Id,
            CheckedAt = entity.CheckedAt,
            Rank = entity.Rank,
            WatchListItemId = entity.WatchListItemId,
        };
    }
}
