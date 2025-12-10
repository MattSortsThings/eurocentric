using System.Net;
using Eurocentric.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.Tests.Acceptance.Features.AdminApi.V0;

[NotInParallel(nameof(AdminApiPingTests))]
public sealed class AdminApiPingTests : AcceptanceTests
{
    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    [Repeat(100)]
    public async Task Ping_should_return_successful_response(string apiVersion)
    {
        // Arrange
        RestRequest request = GetRequest("/admin/api/{apiVersion}/ping").AddUrlSegment("apiVersion", apiVersion);

        // Act
        UnionRestResponse response = await SystemUnderTest.SendRequestAsync(request);

        // Assert
        await Assert
            .That(response.AsSuccessful())
            .IsNotNull()
            .And.HasProperty(restResponse => restResponse.StatusCode, HttpStatusCode.OK);
    }
}
