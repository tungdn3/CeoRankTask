using CeoRankTask.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeoRankTask.Core.Services;

public interface ICeoRankService
{
    Task<IEnumerable<int>> Check(CeoRankRequestDto request);
}
