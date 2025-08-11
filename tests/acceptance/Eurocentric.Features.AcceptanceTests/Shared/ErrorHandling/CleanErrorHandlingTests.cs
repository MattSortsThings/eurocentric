using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using Eurocentric.Features.AcceptanceTests.Utils.Assertions;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class CleanErrorHandlingTests : ParallelCleanAcceptanceTest
{
    [Test]
    public async Task POST_endpoint_should_return_400_with_ProblemDetails_on_missing_required_request_body_property()
    {
        // Arrange
        const string route = "/admin/api/v1.0/countries";

        const string requestBody = """
                                   {
                                    "countryCode": "AT"
                                   }
                                   """;

        RestRequest request = new RestRequest(route, Method.Post).AddJsonBody(requestBody).UseSecretApiKey();

        const string expectedExceptionMessage = "Failed to read parameter \"CreateCountryRequest requestBody\" " +
                                                "from the request body as JSON.";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance("POST /admin/api/v1.0/countries")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task POST_endpoint_should_return_400_with_ProblemDetails_on_invalid_request_body_enum_property_string_value()
    {
        // Arrange
        const string route = "/admin/api/v1.0/contests";

        const string requestBody = """
                                   {
                                     "contestFormat": "INVALID",
                                     "contestYear": 2025,
                                     "cityName": "Basel",
                                     "group1ParticipantData": [
                                       {
                                         "participatingCountryId": "da896d92-9fd7-4757-81bc-2e51ec6aa891",
                                         "ActName": "ActName",
                                         "SongTitle": "SongTitle"
                                       }
                                     ],
                                     "group2ParticipantData": [
                                       {
                                         "participatingCountryId": "12345651-9fd7-4757-81bc-2e51ec6aa891",
                                         "ActName": "ActName",
                                         "SongTitle": "SongTitle"
                                       }
                                     ]
                                   }
                                   """;

        RestRequest request = new RestRequest(route, Method.Post).AddJsonBody(requestBody).UseSecretApiKey();

        const string expectedExceptionMessage = "Failed to read parameter \"CreateContestRequest requestBody\" " +
                                                "from the request body as JSON.";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance("POST /admin/api/v1.0/contests")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task POST_endpoint_should_return_400_with_ProblemDetails_on_invalid_request_body_enum_property_int_value()
    {
        // Arrange
        const string route = "/admin/api/v1.0/contests";

        const string requestBody = """
                                   {
                                     "contestFormat": 999999,
                                     "contestYear": 2025,
                                     "cityName": "Basel",
                                     "group1ParticipantData": [
                                       {
                                         "participatingCountryId": "da896d92-9fd7-4757-81bc-2e51ec6aa891",
                                         "ActName": "ActName",
                                         "SongTitle": "SongTitle"
                                       }
                                     ],
                                     "group2ParticipantData": [
                                       {
                                         "participatingCountryId": "12345651-9fd7-4757-81bc-2e51ec6aa891",
                                         "ActName": "ActName",
                                         "SongTitle": "SongTitle"
                                       }
                                     ]
                                   }
                                   """;

        RestRequest request = new RestRequest(route, Method.Post).AddJsonBody(requestBody).UseSecretApiKey();

        const string expectedExceptionMessage = "The value of argument 'contestFormat' (999999) " +
                                                "is invalid for Enum type 'ContestFormat'. (Parameter 'contestFormat')";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Invalid enum argument")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request.")
            .And.HasInstance("POST /admin/api/v1.0/contests")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task GET_endpoint_should_return_400_with_ProblemDetails_on_missing_required_query_param()
    {
        // Arrange
        const string route = "/public/api/v0.2/rankings/competing-countries/points-in-range";

        RestRequest request = new RestRequest(route).AddQueryParameter("maxPoints", 1).UseSecretApiKey();

        const string expectedExceptionMessage = "Required parameter \"int MinPoints\" was not provided from query string.";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance("GET /public/api/v0.2/rankings/competing-countries/points-in-range?maxPoints=1")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task GET_endpoint_should_return_400_with_ProblemDetails_on_invalid_enum_query_param_string_value()
    {
        // Arrange
        const string route = "/public/api/v0.2/rankings/competing-countries/points-average";

        RestRequest request = new RestRequest(route).AddQueryParameter("votingMethod", "INVALID").UseSecretApiKey();

        const string expectedExceptionMessage = "Failed to bind parameter \"Nullable<QueryableVotingMethod> VotingMethod\"" +
                                                " from \"INVALID\".";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=INVALID")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task GET_endpoint_should_return_400_with_ProblemDetails_on_invalid_enum_query_param_int_value()
    {
        // Arrange
        const string route = "/public/api/v0.2/rankings/competing-countries/points-average";

        RestRequest request = new RestRequest(route).AddQueryParameter("votingMethod", 999999).UseSecretApiKey();

        const string expectedExceptionMessage = "The value of argument 'votingMethod' (999999) is invalid " +
                                                "for Enum type 'QueryableVotingMethod'. (Parameter 'votingMethod')";

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(400)
            .And.HasTitle("Invalid enum argument")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request.")
            .And.HasInstance("GET /public/api/v0.2/rankings/competing-countries/points-average?votingMethod=999999")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage", expectedExceptionMessage);
    }

    [Test]
    public async Task GET_endpoint_should_return_404_with_ProblemDetails_when_non_existent_resource_requested()
    {
        // Arrange
        const string route = "/admin/api/v1.0/countries/fbaf9ff8-bbaa-42ed-a1cc-6b521c9484b9";

        RestRequest request = new RestRequest(route).UseSecretApiKey();

        // Act
        ProblemOrResponse response = await SystemUnderTest.SendAsync(request);

        // Assert
        RestResponse<ProblemDetails> problem = response.AsProblem;

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.NotFound);

        await Assert.That(problem.Data).IsNotNull()
            .And.HasStatus(404)
            .And.HasTitle("Country not found")
            .And.HasDetail("No country exists with the provided country ID.")
            .And.HasInstance("GET /admin/api/v1.0/countries/fbaf9ff8-bbaa-42ed-a1cc-6b521c9484b9")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.5")
            .And.HasExtension("countryId", "fbaf9ff8-bbaa-42ed-a1cc-6b521c9484b9");
    }
}
