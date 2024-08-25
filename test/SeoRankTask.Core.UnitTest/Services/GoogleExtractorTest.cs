using FluentAssertions;
using SeoRankTask.Core.Services;

namespace SeoRankTask.Core.UnitTest.Services;

public class GoogleExtractorTest
{
    [Fact]
    public void ExtractUrls_ShouldReturnCorrectUrlsInOrder()
    {
        var sut = new GoogleExtractor();

        List<string> urls = sut.ExtractUrls(Constants.GoogleHtml);

        var expectedResult = new List<string>
        {
            "https://www.gov.uk",
            "https://www.gov.uk",
            "https://eservices.landregistry.gov.uk",
            "https://www.iris.gov.hk",
            "https://search-property-information.service.gov.uk",
            "http://landregistry.data.gov.uk",
            "https://www.landreg.gov.hk",
            "https://www.landreg.gov.hk",
            "http://www.ontario.ca",
            "https://www.ros.gov.uk"
        };

        urls.Should().Equal(expectedResult);
    }
}
