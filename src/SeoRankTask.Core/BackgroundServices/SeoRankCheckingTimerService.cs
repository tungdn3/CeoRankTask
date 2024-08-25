using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SeoRankTask.Core.Entities;
using SeoRankTask.Core.Interfaces;
using SeoRankTask.Core.Services;

namespace SeoRankTask.Core.BackgroundServices;

public class SeoRankCheckingTimerService : TimerServiceBase
{
    private const string OperationName = "SeoRankChecking";

    private readonly IServiceProvider _serviceProvider;

    public SeoRankCheckingTimerService(
       ILogger<SeoRankCheckingTimerService> logger,
       IConfiguration configuration,
       IServiceProvider serviceProvider)
       : base(logger, configuration["SeoRankCheckingCron"]!, OperationName)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SeoRankCheckingTimer start");

        try
        {
            using var scrope = _serviceProvider.CreateScope();
            var watchListRepository = scrope.ServiceProvider.GetRequiredService<IWatchListRepository>();
            var seoRankService = scrope.ServiceProvider.GetRequiredService<ISeoRankService>();

            List<WatchListItem> items = await watchListRepository.GetAll();
            _logger.LogInformation("Watch list size: {Size}", items.Count);

            foreach (WatchListItem item in items)
            {
                _logger.LogInformation("Process item {ItemId} - {Keyword} - {Url}", item.Id, item.Keyword, item.Url);

                IEnumerable<int> ranks = await seoRankService.Check(new Dtos.SeoRankRequestDto
                {
                    Keyword = item.Keyword,
                    SearchEngine = Enums.SearchEngine.Google,
                    Url = item.Url,
                });

                int minRank = ranks.FirstOrDefault();
                await watchListRepository.SaveHistoricalRank(new HistoricalRank
                {
                    Rank = minRank,
                    CheckedAt = DateTime.UtcNow,
                    WatchListItemId = item.Id
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while checking rank for items in watch list.");
        }

        _logger.LogInformation("SeoRankCheckingTimer end");
    }
}
