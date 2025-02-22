using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Security;

public static class AdminApiKeySecurityTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        private const string Resource = "admin/api/v0.2/calculations/e72fbb74-68ce-4fb3-93db-935c5f2d03b9";

        [Fact]
        public async Task Should_authenticate_and_authorize_request_using_Admin_API_key()
        {
            // Act
            RestRequest request = Get(Resource).AddHeader("X-Api-Key", TestApiKeys.Admin);

            // Act
            RestResponse response = await SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_authenticate_but_not_authorize_request_using_Public_API_key()
        {
            // Act
            RestRequest request = Get(Resource).AddHeader("X-Api-Key", TestApiKeys.Public);

            // Act
            RestResponse response = await SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_unrecognized_API_key()
        {
            // Act
            RestRequest request = Get(Resource).AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

            // Act
            RestResponse response = await SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_no_API_key()
        {
            // Act
            RestRequest request = Get(Resource);

            // Act
            RestResponse response = await SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
