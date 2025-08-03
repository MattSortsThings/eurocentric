using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public static partial class ApiKeySecurityTests
{
    public sealed class AdminApiV0 : SerialCleanAcceptanceTest
    {
        private const string GetContestsRoute = "/admin/api/v0.2/contests";

        [Test]
        public async Task Should_authenticate_request_using_secret_API_key()
        {
            // Arrange
            RestRequest request = new RestRequest(GetContestsRoute)
                .AddHeader("X-Api-Key", TestApiKeys.Secret);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            HttpStatusCode statusCode = result.AsResponse.StatusCode;

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task Should_authenticate_request_using_demo_API_key()
        {
            // Arrange
            RestRequest request = new RestRequest(GetContestsRoute)
                .AddHeader("X-Api-Key", TestApiKeys.Demo);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            HttpStatusCode statusCode = result.AsResponse.StatusCode;

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.OK);
        }

        [Test]
        public async Task Should_not_authenticate_request_using_unrecognized_API_key()
        {
            // Arrange
            RestRequest request = new RestRequest(GetContestsRoute)
                .AddHeader("X-Api-Key", "CECI_N'EST_PAS_UNE_CLE");

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            HttpStatusCode statusCode = result.AsProblem.StatusCode;

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Should_not_authenticate_request_using_no_API_key()
        {
            // Arrange
            RestRequest request = new(GetContestsRoute);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            HttpStatusCode statusCode = result.AsProblem.StatusCode;

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Unauthorized);
        }
    }
}
