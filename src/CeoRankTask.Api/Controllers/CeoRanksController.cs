using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace CeoRankTask.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CeoRanksController : ControllerBase
{
    private readonly ILogger<CeoRanksController> _logger;
    private readonly ICeoRankService _ceoRankService;

    public CeoRanksController(ILogger<CeoRanksController> logger, ICeoRankService ceoRankService)
    {
        _logger = logger;
        _ceoRankService = ceoRankService;
    }

    // Command style, not RESTful
    [HttpPost("check", Name = "CheckCEORank")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<int>> CheckCeoRank([FromBody] CeoRankRequestDto request)
    {
        IEnumerable<int> result = await _ceoRankService.Check(request);
        return result;
    }
}
