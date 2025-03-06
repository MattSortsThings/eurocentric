using System.Net;
using Eurocentric.Shared.Security;
using Eurocentric.Tests.Assertions;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Security;

public static class PublicApiKeySecurityTests
{
    private const string Route = "/public/api/v0.1/stations?line=Jubilee";

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
        public async Task Should_authenticate_and_authorize_request_using_public_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader(ApiKeyConstants.ApiKeyHeader, TestApiKeys.Public);

            // Act
            (HttpStatusCode statusCode, _) = await SendAsync(request);

            // Assert
            statusCode.ShouldBe(HttpStatusCode.OK);
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
