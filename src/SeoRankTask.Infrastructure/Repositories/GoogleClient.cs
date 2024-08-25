using SeoRankTask.Core.Enums;
using SeoRankTask.Core.Exceptions;
using SeoRankTask.Core.Interfaces;

namespace SeoRankTask.Infrastructure.Repositories;

public class GoogleClient : IScraperRepository
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public SearchEngine SearchEngine => SearchEngine.Google;

    public async Task<string> Scrape(string keyword, int top)
    {
        string url = $"search?num={top}&q={keyword}";
        HttpClient httpClient = _httpClientFactory.CreateClient(InfrastructureConstants.HttpClientNames.Google);
        HttpResponseMessage response = await httpClient.GetAsync(url);
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new IntegrationException($"Failed to scrape Google. Url: {url}. Status code: {response.StatusCode}. Content: {content}");
        }

        return content;
    }
}
