using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeoRankTask.Core.Interfaces;

public interface IScraperRepository
{
    Task<string> ScrapeGoogle(string keyword);
}
