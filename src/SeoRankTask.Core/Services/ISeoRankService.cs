using CeoRankTask.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeoRankTask.Core.Services;

public interface ISeoRankService
{
    Task<IEnumerable<int>> Check(SeoRankRequestDto request);
}
