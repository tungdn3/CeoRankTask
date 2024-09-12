using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Interfaces;

namespace SeoRankTask.Core.Services;

public class SeoRankService : ISeoRankService
{
    private const int Top = 100;

    private readonly ILogger<SeoRankService> _logger;
    private readonly IValidator<SeoRankRequestDto> _seoRankRequestDtoValidator;
    private readonly IEnumerable<IExtractor> _extractors;
    private readonly IEnumerable<IScraperClient> _scraperClients;

    public SeoRankService(
        ILogger<SeoRankService> logger,
        IValidator<SeoRankRequestDto> seoRankRequestValidator,
        IEnumerable<IExtractor> extractors,
        IEnumerable<IScraperClient> scraperClients)
    {
        _logger = logger;
        _seoRankRequestDtoValidator = seoRankRequestValidator;
        _extractors = extractors;
        _scraperClients = scraperClients;
    }

    public async Task<IEnumerable<int>> Check(SeoRankRequestDto request)
    {
        ValidationResult result = await _seoRankRequestDtoValidator.ValidateAsync(request);

        if (!result.IsValid)
        {
            throw new Exceptions.ValidationException(result.Errors);
        }

        IScraperClient scraper = _scraperClients.First(x => x.SearchEngine == request.SearchEngine);
        string rawHtml = await scraper.Scrape(request.Keyword, Top);

        IExtractor extractor = _extractors.First(x => x.SearchEngine == request.SearchEngine);
        List<string> urls = extractor.ExtractUrls(rawHtml);

        string baseUrl = GetBaseUrlWithoutTrailingSlash(request.Url);

        var ranks = new List<int>();
        for (int i = 0; i < urls.Count; i++)
        {
            if (urls[i] == baseUrl)
            {
                ranks.Add(i + 1);
            }
        }

        return ranks;
    }

    private static string GetBaseUrlWithoutTrailingSlash(string url)
    {
        Uri uri = url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
            ? new Uri(url)
            : new Uri($"https://{url}");

        return uri.Scheme + "://" + uri.Authority;
    }
}
