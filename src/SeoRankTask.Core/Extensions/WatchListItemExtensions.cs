using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Entities;

namespace SeoRankTask.Core.Extensions;

public static class WatchListItemExtensions
{
    public static WatchListItemDto ToDto(this WatchListItem entity)
    {
        return new WatchListItemDto
        {
            Id = entity.Id,
            Keyword = entity.Keyword,
            Url = entity.Url,
        };
    }
}
