using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CeoRankTask.Core.Services;

public class SeoRankService : ISeoRankService
{
    private const int Top = 100;

    private readonly ILogger<SeoRankService> _logger;
    private readonly IValidator<SeoRankRequestDto> _ceoRankRequestDtoValidator;
    private readonly IEnumerable<IExtractor> _extractors;
    private readonly IEnumerable<IScraperRepository> _scraperRepositories;

    public SeoRankService(
        ILogger<SeoRankService> logger,
        IValidator<SeoRankRequestDto> ceoRankRequestValidator,
        IEnumerable<IExtractor> extractors,
        IEnumerable<IScraperRepository> scraperRepositories)
    {
        _logger = logger;
        _ceoRankRequestDtoValidator = ceoRankRequestValidator;
        _extractors = extractors;
        _scraperRepositories = scraperRepositories;
    }

    public async Task<IEnumerable<int>> Check(SeoRankRequestDto request)
    {
        ValidationResult result = await _ceoRankRequestDtoValidator.ValidateAsync(request);

        if (!result.IsValid)
        {
            throw new Exceptions.ValidationException(result.Errors);
        }

        IScraperRepository scraper = _scraperRepositories.First(x => x.SearchEngine == request.SearchEngine);
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
