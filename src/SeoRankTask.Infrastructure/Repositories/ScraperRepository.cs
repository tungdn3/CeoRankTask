using CeoRankTask.Core.Exceptions;
using CeoRankTask.Core.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeoRankTask.Infrastructure.Repositories;

public class ScraperRepository : IScraperRepository
{
    private readonly HttpClient _httpClient;
    private readonly ScraperRepositoryOptions _options;

    public ScraperRepository(HttpClient httpClient, IOptions<ScraperRepositoryOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _httpClient.BaseAddress = new Uri(_options.GoogleBaseUrl);
    }

    public async Task<string> ScrapeGoogle(string keyword)
    {
        string url = $"search?num={_options.MaxRank}&q={keyword}";
        HttpResponseMessage response = await _httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new IntegrationException($"Failed to scrape Google. Url: {url}. Status code: {response.StatusCode}. Content: {content}");
        }

        return content;
    }
}
