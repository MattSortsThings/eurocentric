using System.Net;
using Eurocentric.Shared.Security;
using Eurocentric.Tests.Assertions;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Security;

public static class AdminApiKeySecurityTests
{
    private const string Route = "/admin/api/v0.1/calculations/4aa31026-0cf6-4006-903d-cfe6d328b96f";

    public sealed class AdminApi(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_authenticate_and_authorize_request_using_admin_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Admin);

            // Act
            (HttpStatusCode statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Should_authenticate_but_not_authorize_request_using_public_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Public);

            // Act
            (HttpStatusCode statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_unrecognized_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Unrecognized);

            // Act
            (HttpStatusCode statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_no_API_key()
        {
            // Arrange
            RestRequest request = Get(Route);

            // Act
            (HttpStatusCode statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
