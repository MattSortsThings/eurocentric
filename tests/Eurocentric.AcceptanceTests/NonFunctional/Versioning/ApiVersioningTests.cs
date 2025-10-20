using Eurocentric.AcceptanceTests.TestUtils;
using Eurocentric.AcceptanceTests.TestUtils.Assertions;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.Versioning;

[Category("versioning")]
public sealed class ApiVersioningTests : ParallelSeededAcceptanceTest
{
    [Test]
    public async Task Admin_API_v0_1_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/v0.1/countries");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            request,
            TestContext.Current!.CancellationToken
        );

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Admin_API_v0_2_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/v0.2/countries");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            request,
            TestContext.Current!.CancellationToken
        );

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Public_API_v0_1_response_should_report_all_Public_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/public/api/v0.1/queryables/countries");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            request,
            TestContext.Current!.CancellationToken
        );

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Public_API_v0_2_response_should_report_all_Public_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/public/api/v0.2/queryables/countries");

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(
            request,
            TestContext.Current!.CancellationToken
        );

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2");
    }

    private static RestRequest GetRequest(string route) => new(route);
}
