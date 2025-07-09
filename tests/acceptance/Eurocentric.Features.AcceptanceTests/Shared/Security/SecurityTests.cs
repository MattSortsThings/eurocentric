using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public static class SecurityTests
{
    public sealed class AdminApiV0(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        private const string Route = "/admin/api/v0.2/contests";

        [Fact]
        public async Task Should_authenticate_request_using_secret_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", TestApiKeys.SecretApiKey);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsResponse.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Should_authenticate_request_using_demo_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", TestApiKeys.DemoApiKey);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsResponse.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_unrecognized_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", "THIS_IS_NOT_A_KEY");

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_no_API_key()
        {
            // Arrange
            RestRequest request = Get(Route);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
    }

    public sealed class PublicApiV0(WebAppFixture fixture) : AcceptanceTest(fixture)
    {
        private const string Route = "/public/api/v0.2/filters/contest-stages";

        [Fact]
        public async Task Should_authenticate_request_using_secret_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", TestApiKeys.SecretApiKey);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsResponse.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Should_authenticate_request_using_demo_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", TestApiKeys.DemoApiKey);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsResponse.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_unrecognized_API_key()
        {
            // Arrange
            RestRequest request = Get(Route).AddHeader("X-Api-Key", "THIS_IS_NOT_A_KEY");

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }

        [Fact]
        public async Task Should_not_authenticate_request_using_no_API_key()
        {
            // Arrange
            RestRequest request = Get(Route);

            // Act
            ProblemOrResponse problemOrResponse = await RestClient.SendAsync(request, TestContext.Current.CancellationToken);

            HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
        }
    }
}
