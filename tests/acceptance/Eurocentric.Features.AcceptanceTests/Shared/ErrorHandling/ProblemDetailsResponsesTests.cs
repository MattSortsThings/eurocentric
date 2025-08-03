using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Contests.CreateContest;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public static class ProblemDetailsResponsesTests
{
    private static async Task CreateAContestWithContestYearAsync(this IWebAppFixtureRestClient sut, int contestYear)
    {
        RestRequest createContestRequest = new RestRequest("/admin/api/v0.2/contests", Method.Post)
            .AddJsonBody(DefaultCreateContestRequest() with { ContestYear = contestYear });

        _ = await sut.SendAsync(createContestRequest);
    }

    private static CreateContestRequest DefaultCreateContestRequest() => new()
    {
        ContestYear = 2025,
        CityName = "CityName",
        ContestFormat = ContestFormat.Liverpool,
        ParticipatingCountryIds = Enumerable.Range(0, 3).Select(_ => Guid.NewGuid()).ToArray()
    };

    public sealed class Endpoints : SerialCleanAcceptanceTest
    {
        [Test]
        public async Task Should_return_404_with_ProblemDetails_when_request_references_non_existent_resource()
        {
            // Arrange
            Guid contestId = Guid.Parse("d730ad20-3d70-4160-8849-c4c52f5752e0");

            RestRequest request = new RestRequest("/admin/api/v0.2/contests/{contestId}")
                .AddUrlSegment("contestId", contestId);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.NotFound);

            await Assert.That(problemDetails.Status).IsEqualTo(404);
            await Assert.That(problemDetails.Title).IsEqualTo("Contest not found");
            await Assert.That(problemDetails.Detail).IsEqualTo("No contest exists with the provided contest ID.");
            await Assert.That(problemDetails.Instance)
                .IsEqualTo("GET /admin/api/v0.2/contests/d730ad20-3d70-4160-8849-c4c52f5752e0");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.5");
            await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "contestId", Value: JsonElement je }
                                                                         && je.GetGuid() == contestId);
        }

        [Test]
        public async Task Should_return_409_with_ProblemDetails_when_request_is_illegal_given_system_state()
        {
            // Arrange
            const int sharedContestYear = 2016;
            await SystemUnderTest.CreateAContestWithContestYearAsync(sharedContestYear);

            CreateContestRequest requestBody = DefaultCreateContestRequest() with { ContestYear = sharedContestYear };

            RestRequest request = new RestRequest("/admin/api/v0.2/contests", Method.Post)
                .AddJsonBody(requestBody);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Conflict);

            await Assert.That(problemDetails.Status).IsEqualTo(409);
            await Assert.That(problemDetails.Title).IsEqualTo("Contest year conflict");
            await Assert.That(problemDetails.Detail).IsEqualTo("A contest already exists with the provided contest year.");
            await Assert.That(problemDetails.Instance).IsEqualTo("POST /admin/api/v0.2/contests");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.10");
            await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "contestYear", Value: JsonElement je }
                                                                         && je.GetInt32() == sharedContestYear);
        }

        [Test]
        public async Task Should_return_422_with_ProblemDetails_when_request_is_intrinsically_illegal()
        {
            // Arrange
            const int illegalContestYear = 1066;

            CreateContestRequest requestBody = DefaultCreateContestRequest() with { ContestYear = illegalContestYear };

            RestRequest request = new RestRequest("/admin/api/v0.2/contests", Method.Post)
                .AddJsonBody(requestBody);

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);

            await Assert.That(problemDetails.Status).IsEqualTo(422);
            await Assert.That(problemDetails.Title).IsEqualTo("Illegal contest year value");
            await Assert.That(problemDetails.Detail).IsEqualTo("Contest year value must be an integer between 2016 and 2050.");
            await Assert.That(problemDetails.Instance).IsEqualTo("POST /admin/api/v0.2/contests");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc4918#section-11.2");
            await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "contestYear", Value: JsonElement je }
                                                                         && je.GetInt32() == illegalContestYear);
        }
    }
}
