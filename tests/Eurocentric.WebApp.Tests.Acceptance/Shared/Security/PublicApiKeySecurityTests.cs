using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.Security;

public static class PublicApiKeySecurityTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        private const string Resource = "public/api/v0.1/stations?line=Northern";

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
        public async Task Should_authenticate_and_authorize_request_using_Public_API_key()
        {
            // Act
            RestRequest request = Get(Resource).AddHeader("X-Api-Key", TestApiKeys.Public);

            // Act
            RestResponse response = await SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
