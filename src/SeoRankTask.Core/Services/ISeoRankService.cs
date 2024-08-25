using SeoRankTask.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeoRankTask.Core.Services;

public interface ISeoRankService
{
    Task<IEnumerable<int>> Check(SeoRankRequestDto request);
}
