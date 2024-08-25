using Microsoft.AspNetCore.Mvc;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Services;

namespace SeoRankTask.Api.Controllers;

[ApiController]
[Route("watch-list")]
public class WatchListController : ControllerBase
{
    private readonly ILogger<WatchListController> _logger;
    private readonly IWatchListService _watchListService;

    public WatchListController(ILogger<WatchListController> logger, IWatchListService watchListService)
    {
        _logger = logger;
        _watchListService = watchListService;
    }

    [HttpGet(Name = "GetWatchList")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<PageResultDto<WatchListItemDto>> Get(int pageSize = 10, int pageNumber = 1)
    {
        PageResultDto<WatchListItemDto> result = await _watchListService.Get(pageSize, pageNumber);
        return result;
    }

    [HttpGet("{id}/historical-ranks", Name = "GetHistoricalRanksByWatchListItem")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<HistoricalRankDto>> GetHistoricalRanksByWatchListItem(int id, DateTime? dateFrom = null)
    {
        IEnumerable<HistoricalRankDto> result = await _watchListService.GetHistoricalRanks(id, dateFrom);
        return result;
    }
}
