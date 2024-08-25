using Microsoft.AspNetCore.Mvc;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Services;

namespace SeoRankTask.Api.Controllers;

[ApiController]
[Route("seo-ranks")]
public class SeoRanksController : ControllerBase
{
    private readonly ILogger<SeoRanksController> _logger;
    private readonly ISeoRankService _ceoRankService;

    public SeoRanksController(ILogger<SeoRanksController> logger, ISeoRankService ceoRankService)
    {
        _logger = logger;
        _ceoRankService = ceoRankService;
    }

    // Command style, not RESTful
    [HttpPost("check", Name = "CheckCEORank")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<int>> CheckCeoRank([FromBody] SeoRankRequestDto request)
    {
        IEnumerable<int> result = await _ceoRankService.Check(request);
        return result;
    }
}
