using SeoRankTask.Core.Dtos;

namespace SeoRankTask.Core.Services;

public interface ISeoRankService
{
    Task<IEnumerable<int>> Check(SeoRankRequestDto request);
}
