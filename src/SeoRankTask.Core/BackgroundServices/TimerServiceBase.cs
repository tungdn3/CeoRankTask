using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NCrontab;
using static NCrontab.CrontabSchedule;

namespace SeoRankTask.Core.BackgroundServices;

public abstract class TimerServiceBase : BackgroundService
{
    private readonly CrontabSchedule _schedule;
    protected readonly ILogger _logger;
    protected string _operationName;

    protected TimerServiceBase(ILogger logger, string cron, string operationName = "Timer")
    {
        _logger = logger;
        _schedule = Parse(cron);
        _operationName = operationName;
    }

    public DateTime NextRun { get; private set; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            NextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
            _logger.LogWarning("{OperationName}, next run: {NextRun} UTC.", _operationName, NextRun);
            while (!stoppingToken.IsCancellationRequested)
            {
                TimeSpan diff = NextRun - DateTime.UtcNow;
                if (diff <= TimeSpan.Zero)
                {
                    await ExecuteTaskAsync(stoppingToken);
                    NextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
                    _logger.LogWarning("{OperationName}, next run: {NextRun} UTC.", _operationName, NextRun);
                }

                diff = NextRun - DateTime.UtcNow;
                await Task.Delay((int)diff.TotalMilliseconds, stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Timer service - Error occurred while executing {OperationName}.", _operationName);
            Environment.Exit(1);
        }
    }

    protected abstract Task ExecuteTaskAsync(CancellationToken stoppingToken);
}
