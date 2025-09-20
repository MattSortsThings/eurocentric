using Eurocentric.WebApp.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.WebApp.AcceptanceTests.Versioning;

public sealed class ApiVersionReportingTests : SeededParallelAcceptanceTest
{
    [Test]
    public async Task Admin_API_V0Point1_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        const string getCountryRoute = "/admin/api/v0.1/countries/{countryId}";

        Guid seededCountryId = Guid.Parse("01979615-1E4C-7BA1-868B-018CE12E1C0C");

        RestRequest request = GetRequest(getCountryRoute)
            .AddUrlSegment("countryId", seededCountryId);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        string? headerValue = response.AsSuccessful.GetHeaderValue("api-supported-versions");

        await Assert.That(headerValue).IsEqualTo("0.1, 0.2");
    }

    [Test]
    public async Task Admin_API_V0Point2_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        const string getCountryRoute = "/admin/api/v0.2/countries/{countryId}";

        Guid seededCountryId = Guid.Parse("01979615-1E4C-7BA1-868B-018CE12E1C0C");

        RestRequest request = GetRequest(getCountryRoute)
            .AddUrlSegment("countryId", seededCountryId);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        string? headerValue = response.AsSuccessful.GetHeaderValue("api-supported-versions");

        await Assert.That(headerValue).IsEqualTo("0.1, 0.2");
    }

    [Test]
    public async Task Public_API_V0Point1_response_should_report_all_Public_API_versions()
    {
        // Arrange
        const string getQueryableContestsRoute = "/public/api/v0.1/queryables/contests";

        RestRequest request = GetRequest(getQueryableContestsRoute);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        string? headerValue = response.AsSuccessful.GetHeaderValue("api-supported-versions");

        await Assert.That(headerValue).IsEqualTo("0.1, 0.2");
    }

    [Test]
    public async Task Public_API_V0Point2_response_should_report_all_Public_API_versions()
    {
        // Arrange
        const string getQueryableContestsRoute = "/public/api/v0.2/queryables/contests";

        RestRequest request = GetRequest(getQueryableContestsRoute);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        string? headerValue = response.AsSuccessful.GetHeaderValue("api-supported-versions");

        await Assert.That(headerValue).IsEqualTo("0.1, 0.2");
    }

    private static RestRequest GetRequest(string route) => new(route);
}
