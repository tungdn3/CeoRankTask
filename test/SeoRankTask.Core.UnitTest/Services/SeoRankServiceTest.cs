using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using SeoRankTask.Core.Dtos;
using SeoRankTask.Core.Interfaces;
using SeoRankTask.Core.Services;

namespace SeoRankTask.Core.UnitTest.Services;

public class SeoRankServiceTest
{
    private readonly ILogger<SeoRankService> _logger = new Logger<SeoRankService>(new NullLoggerFactory());
    private readonly CeoRankRequestDtoValidator _validator = new ();
    private readonly Mock<IScraperRepository> _googleClientMock = new();
    private readonly Mock<IExtractor> _googleExtractorMock = new();
    private readonly IEnumerable<IExtractor> _extractors;
    private readonly IEnumerable<IScraperRepository> _scraperRepositories;

    public SeoRankServiceTest()
    {
        _googleClientMock.Setup(x => x.SearchEngine).Returns(Enums.SearchEngine.Google);
        _extractors = new List<IExtractor>
        {
            _googleExtractorMock.Object,
        };

        _googleClientMock.Setup(x => x.SearchEngine).Returns(Enums.SearchEngine.Google);
        _scraperRepositories = new List<IScraperRepository>
        {
            _googleClientMock.Object,
        };
    }

    [Fact]
    public async Task Check_ShouldReturnListOfRanks()
    {
        var request = new SeoRankRequestDto
        {
            Keyword = "test",
            SearchEngine = Enums.SearchEngine.Google,
            Url = "www.my-website.com"
        };

        string googleHtml = "test";
        _googleClientMock.Setup(x => x.Scrape(request.Keyword, 100))
            .ReturnsAsync(googleHtml);

        _googleExtractorMock.Setup(x => x.ExtractUrls(googleHtml))
            .Returns(new List<string>
            {
                "https://test1.com",
                "https://www.my-website.com",
                "https://www.my-website.com",
                "https://test2.com",
            });

        var sut = new SeoRankService(
            _logger,
            _validator,
            _extractors,
            _scraperRepositories);

        IEnumerable<int> ranks = await sut.Check(request);

        ranks.Should().Equal(new List<int> { 2, 3 });
    }
}
