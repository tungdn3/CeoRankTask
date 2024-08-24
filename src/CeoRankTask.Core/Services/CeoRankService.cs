using CeoRankTask.Core.Dtos;
using CeoRankTask.Core.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace CeoRankTask.Core.Services;

public class CeoRankService : ICeoRankService
{
    private readonly ILogger<CeoRankService> _logger;
    private readonly IValidator<CeoRankRequestDto> _ceoRankRequestDtoValidator;
    private readonly IGoogleExtractor _googleExtractor;
    private readonly IScraperRepository _scraperRepository;

    public CeoRankService(
        ILogger<CeoRankService> logger,
        IValidator<CeoRankRequestDto> ceoRankRequestValidator,
        IGoogleExtractor googleExtractor,
        IScraperRepository scraperRepository)
    {
        _logger = logger;
        _ceoRankRequestDtoValidator = ceoRankRequestValidator;
        _googleExtractor = googleExtractor;
        _scraperRepository = scraperRepository;
    }

    public async Task<IEnumerable<int>> Check(CeoRankRequestDto request)
    {
        ValidationResult result = await _ceoRankRequestDtoValidator.ValidateAsync(request);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }

        string rawHtml = await _scraperRepository.ScrapeGoogle(request.Keyword);
        List<string> urls = _googleExtractor.ExtractUrls(rawHtml);

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
