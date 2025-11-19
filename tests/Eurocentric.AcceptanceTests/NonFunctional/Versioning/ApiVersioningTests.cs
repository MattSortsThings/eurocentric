using Eurocentric.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.Versioning;

[Category("versioning")]
public sealed class ApiVersioningTests : ParallelSeededAcceptanceTest
{
    [Test]
    public async Task Admin_API_v0_1_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/v0.1/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert
            .That(problemOrResponse)
            .IsResponse()
            .And.HasHeader("api-supported-versions", "1.0")
            .And.HasHeader("api-deprecated-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Admin_API_v0_2_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/v0.2/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert
            .That(problemOrResponse)
            .IsResponse()
            .And.HasHeader("api-supported-versions", "1.0")
            .And.HasHeader("api-deprecated-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Admin_API_v1_0_response_should_report_all_Admin_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/v1.0/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert
            .That(problemOrResponse)
            .IsResponse()
            .And.HasHeader("api-supported-versions", "1.0")
            .And.HasHeader("api-deprecated-versions", "0.1, 0.2");
    }

    [Test]
    public async Task Public_API_v0_1_response_should_report_all_Public_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/public/api/v0.1/queryables/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2, 1.0");
    }

    [Test]
    public async Task Public_API_v0_2_response_should_report_all_Public_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/public/api/v0.2/queryables/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2, 1.0");
    }

    [Test]
    public async Task Public_API_v1_0_response_should_report_all_Public_API_versions()
    {
        // Arrange
        RestRequest request = GetRequest("/public/api/v1.0/queryables/countries").UseSecretApiKey();

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(request);

        // Assert
        await Assert.That(problemOrResponse).IsResponse().And.HasHeader("api-supported-versions", "0.1, 0.2, 1.0");
    }

    private static RestRequest GetRequest(string route) => new(route);
}
