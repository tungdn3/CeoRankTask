using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CeoRankTask.Api.Controllers;

[ApiController]
[Route("[controller]")]
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
