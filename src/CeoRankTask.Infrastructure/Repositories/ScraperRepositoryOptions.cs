namespace CeoRankTask.Infrastructure.Repositories;

public class ScraperRepositoryOptions
{
    public const string ScraperRepository = "ScraperRepository";

    public string GoogleBaseUrl { get; set; } = string.Empty;

    public int MaxRank { get; set; } = 100;
}
