using CeoRankTask.Core.Exceptions;
using CeoRankTask.Core.Interfaces;
using SeoRankTask.Core.Enums;

namespace SeoRankTask.Infrastructure.Repositories;

public class BingClient : IScraperRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BingClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public SearchEngine SearchEngine => SearchEngine.Bing;

    public async Task<string> Scrape(string keyword, int top)
    {
        const int BingPageSize = 10;

        List<Task<string>> tasks = new List<Task<string>>();
        int from = 1;
        while (from < top)
        {
            tasks.Add(ScrapeBing(keyword, from));
            from += BingPageSize;
        }

        string[] results = await Task.WhenAll(tasks);
        string bigHtml = string.Join(',', results);
        return bigHtml;
    }

    private async Task<string> ScrapeBing(string keyword, int from)
    {
        string url = $"search?q={keyword}&first={from}";
        HttpClient httpClient = _httpClientFactory.CreateClient(InfrastructureConstants.HttpClientNames.Bing);
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new IntegrationException($"Failed to scrape Bing. Url: {url}. Status code: {response.StatusCode}. Content: {content}");
        }

        return content;
    }
}
