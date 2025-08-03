using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public static class ExceptionHandlingTests
{
    public sealed class Endpoints : SerialCleanAcceptanceTest
    {
        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_missing_required_request_body_property()
        {
            // Arrange
            const string requestBody = """
                                       {
                                        "cityName": "CityName",
                                        "contestYear": 2023,
                                        "participatingCountryIds": [
                                            "d3b2cf36-e5f6-4209-a2bf-b8248f8838e1",
                                            "95ac0873-0db1-4b3a-902f-33c7f09fef5d"
                                        ]
                                       }
                                       """;

            RestRequest request = new RestRequest("/admin/api/v0.2/contests", Method.Post)
                .AddJsonBody(requestBody);

            const string expectedExceptionMessage = "Failed to read parameter \"CreateContestRequest requestBody\" " +
                                                    "from the request body as JSON.";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Bad HTTP request");
            await Assert.That(problemDetails.Detail).IsEqualTo("BadHttpRequestException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance).IsEqualTo("POST /admin/api/v0.2/contests");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_enum_request_body_property_with_invalid_string_value()
        {
            // Arrange
            const string requestBody = """
                                       {
                                        "contestFormat": "NOT_A_CONTEST_FORMAT",
                                        "cityName": "CityName",
                                        "contestYear": 2023,
                                        "participatingCountryIds": [
                                            "d3b2cf36-e5f6-4209-a2bf-b8248f8838e1",
                                            "95ac0873-0db1-4b3a-902f-33c7f09fef5d"
                                        ]
                                       }
                                       """;

            RestRequest request = new RestRequest("/admin/api/v0.2/contests", Method.Post)
                .AddJsonBody(requestBody);

            const string expectedExceptionMessage = "Failed to read parameter \"CreateContestRequest requestBody\" " +
                                                    "from the request body as JSON.";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Bad HTTP request");
            await Assert.That(problemDetails.Detail).IsEqualTo("BadHttpRequestException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance).IsEqualTo("POST /admin/api/v0.2/contests");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_enum_request_body_property_with_invalid_int_value()
        {
            // Arrange
            const string requestBody = """
                                       {
                                        "contestFormat": 999999,
                                        "cityName": "CityName",
                                        "contestYear": 2023,
                                        "participatingCountryIds": [
                                            "d3b2cf36-e5f6-4209-a2bf-b8248f8838e1",
                                            "95ac0873-0db1-4b3a-902f-33c7f09fef5d"
                                        ]
                                       }
                                       """;

            RestRequest request = new RestRequest("/admin/api/v0.2/contests", Method.Post)
                .AddJsonBody(requestBody);

            const string expectedExceptionMessage = "The value of argument 'contestFormat' (999999) " +
                                                    "is invalid for Enum type 'ContestFormat'. (Parameter 'contestFormat')";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Invalid enum argument");
            await Assert.That(problemDetails.Detail)
                .IsEqualTo("InvalidEnumArgumentException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance).IsEqualTo("POST /admin/api/v0.2/contests");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_missing_required_query_param()
        {
            // Arrange
            RestRequest request = new RestRequest("/public/api/v0.2/rankings/competing-countries/points-in-range")
                .AddQueryParameter("maxPoints", 12);

            const string expectedExceptionMessage = "Required parameter \"int MinPoints\" was not provided from query string.";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Bad HTTP request");
            await Assert.That(problemDetails.Detail).IsEqualTo("BadHttpRequestException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance)
                .IsEqualTo("GET /public/api/v0.2/rankings/competing-countries/points-in-range?maxPoints=12");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_enum_query_param_with_invalid_string_value()
        {
            // Arrange
            RestRequest request = new RestRequest("/public/api/v0.2/rankings/competing-countries/points-average")
                .AddQueryParameter("votingMethod", "NOT_A_VOTING_METHOD");

            const string expectedExceptionMessage = "Failed to bind parameter \"Nullable<QueryableVotingMethod> VotingMethod\"" +
                                                    " from \"NOT_A_VOTING_METHOD\".";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Bad HTTP request");
            await Assert.That(problemDetails.Detail).IsEqualTo("BadHttpRequestException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance)
                .IsEqualTo("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=NOT_A_VOTING_METHOD");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_400_with_ProblemDetails_given_enum_query_param_with_invalid_int_value()
        {
            // Arrange
            RestRequest request = new RestRequest("/public/api/v0.2/rankings/competing-countries/points-average")
                .AddQueryParameter("votingMethod", 999999);

            const string expectedExceptionMessage = "The value of argument 'votingMethod' (999999) is invalid " +
                                                    "for Enum type 'QueryableVotingMethod'. (Parameter 'votingMethod')";

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            var (statusCode, problemDetails) = (result.AsProblem.StatusCode, result.AsProblem.Data!);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

            await Assert.That(problemDetails.Status).IsEqualTo(400);
            await Assert.That(problemDetails.Title).IsEqualTo("Invalid enum argument");
            await Assert.That(problemDetails.Detail)
                .IsEqualTo("InvalidEnumArgumentException was thrown while handling the request.");
            await Assert.That(problemDetails.Instance)
                .IsEqualTo("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=999999");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.5.1");
            await Assert.That(problemDetails.Extensions).Contains(kvp =>
                kvp is { Key: "exceptionMessage", Value: JsonElement je }
                && je.GetString() == expectedExceptionMessage);
        }

        [Test]
        public async Task Should_return_503_with_ProblemDetails_given_database_timeout_while_handling_request_using_EF_Core()
        {
            // Arrange
            await SystemUnderTest.PauseDbContainerAsync();

            RestRequest request = new("/public/api/v0.2/queryables/countries");

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails, IReadOnlyCollection<HeaderParameter>? headers) =
                (result.AsProblem.StatusCode, result.AsProblem.Data!, result.AsProblem.Headers);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);

            await Assert.That(problemDetails.Status).IsEqualTo(503);
            await Assert.That(problemDetails.Title).IsEqualTo("Database timeout");
            await Assert.That(problemDetails.Detail)
                .IsEqualTo("SqlException was thrown while handling the request because the database connection " +
                           "or operation timed out. Please retry after c.120 seconds.");
            await Assert.That(problemDetails.Instance).IsEqualTo("GET /public/api/v0.2/queryables/countries");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.6.4");

            await Assert.That(headers).Contains(h => h is { Name: "Retry-After", Value: "120" });
        }

        [Test]
        public async Task Should_return_503_with_ProblemDetails_given_database_timeout_while_handling_request_using_Dapper()
        {
            // Arrange
            await SystemUnderTest.PauseDbContainerAsync();

            RestRequest request = new("/public/api/v0.2/rankings/competing-countries/points-average");

            // Act
            ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails, IReadOnlyCollection<HeaderParameter>? headers) =
                (result.AsProblem.StatusCode, result.AsProblem.Data!, result.AsProblem.Headers);

            await Assert.That(statusCode).IsEqualTo(HttpStatusCode.ServiceUnavailable);

            await Assert.That(problemDetails.Status).IsEqualTo(503);
            await Assert.That(problemDetails.Title).IsEqualTo("Database timeout");
            await Assert.That(problemDetails.Detail)
                .IsEqualTo("SqlException was thrown while handling the request because the database connection " +
                           "or operation timed out. Please retry after c.120 seconds.");
            await Assert.That(problemDetails.Instance)
                .IsEqualTo("GET /public/api/v0.2/rankings/competing-countries/points-average");
            await Assert.That(problemDetails.Type).IsEqualTo("https://tools.ietf.org/html/rfc9110#section-15.6.4");

            await Assert.That(headers).Contains(h => h is { Name: "Retry-After", Value: "120" });
        }
    }
}
