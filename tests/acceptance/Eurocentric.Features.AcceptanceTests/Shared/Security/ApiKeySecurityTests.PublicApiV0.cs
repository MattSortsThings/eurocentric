using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public static partial class ApiKeySecurityTests
{
    public sealed class PublicApiV0 : SerialCleanAcceptanceTest
    {
        private const string GetQueryableContestStagesRoute = "/public/api/v0.2/queryables/contest-stages";

        [Test]
        public async Task Should_authenticate_request_using_secret_API_key()
        {
            // Arrange
            RestRequest request = new RestRequest(GetQueryableContestStagesRoute)
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
            RestRequest request = new RestRequest(GetQueryableContestStagesRoute)
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
            RestRequest request = new RestRequest(GetQueryableContestStagesRoute)
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
            RestRequest request = new(GetQueryableContestStagesRoute);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            HttpStatusCode statusCode = result.AsProblem.StatusCode;

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Unauthorized);
        }
    }
}
