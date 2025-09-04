using System.Net;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils.Assertions;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ErrorHandlingTests : ParallelSeededAcceptanceTest
{
    private const string CreateCountryRoute = "/admin/api/v0.2/countries";
    private const string GetCompetingCountryPointsInRangeRankingsRoute =
        "/public/api/v0.2/rankings/competing-countries/points-in-range";

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_missing_required_request_body_property()
    {
        // Arrange
        object requestBodyMissingCountryCode = new { CountryName = "CountryName", CountryType = "Real" };

        RestRequest request = new RestRequest(CreateCountryRoute, Method.Post).AddJsonBody(requestBodyMissingCountryCode)
            .UseSecretApiKey();

        const string expectedInstance = "POST " + CreateCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage",
                "Failed to read parameter \"CreateCountryRequest requestBody\" from the request body as JSON.");
    }

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_invalid_request_body_enum_property_string_value()
    {
        // Arrange
        object requestBodyInvalidCountryType = new
        {
            CountryCode = "AA", CountryName = "CountryName", CountryType = "NOT_A_COUNTRY_TYPE"
        };

        RestRequest request = new RestRequest(CreateCountryRoute, Method.Post).AddJsonBody(requestBodyInvalidCountryType)
            .UseSecretApiKey();

        const string expectedInstance = "POST " + CreateCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage",
                "Failed to read parameter \"CreateCountryRequest requestBody\" from the request body as JSON.");
    }

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_invalid_request_body_enum_property_int_value()
    {
        // Arrange
        object requestBodyInvalidCountryType = new { CountryCode = "AA", CountryName = "CountryName", CountryType = 999 };

        RestRequest request = new RestRequest(CreateCountryRoute, Method.Post).AddJsonBody(requestBodyInvalidCountryType)
            .UseSecretApiKey();

        const string expectedInstance = "POST " + CreateCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request. " +
                           "Please use valid string enum values.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1");
    }

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_missing_required_query_string_param()
    {
        // Arrange
        const string queryStringMissingMaxPoints = "?minPoints=0";

        RestRequest request = new RestRequest(GetCompetingCountryPointsInRangeRankingsRoute + queryStringMissingMaxPoints)
            .UseSecretApiKey();

        const string expectedInstance = "GET " + GetCompetingCountryPointsInRangeRankingsRoute + queryStringMissingMaxPoints;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage",
                "Required parameter \"int MaxPoints\" was not provided from query string.");
    }

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_invalid_query_string_enum_param_string_value()
    {
        // Arrange
        const string queryStringInvalidVotingMethod = "?minPoints=0&maxPoints=0&votingMethod=NOT_A_VOTING_METHOD";

        RestRequest request = new RestRequest(GetCompetingCountryPointsInRangeRankingsRoute + queryStringInvalidVotingMethod)
            .UseSecretApiKey();

        const string expectedInstance = "GET " + GetCompetingCountryPointsInRangeRankingsRoute + queryStringInvalidVotingMethod;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("BadHttpRequestException was thrown while handling the request.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1")
            .And.HasExtension("exceptionMessage",
                "Failed to bind parameter \"Nullable<QueryableVotingMethod> VotingMethod\" from \"NOT_A_VOTING_METHOD\".");
    }

    [Test]
    public async Task Endpoint_should_return_400_with_ProblemDetails_on_invalid_query_string_enum_param_int_value()
    {
        // Arrange
        const string queryStringInvalidVotingMethod = "?minPoints=0&maxPoints=0&votingMethod=999";

        RestRequest request = new RestRequest(GetCompetingCountryPointsInRangeRankingsRoute + queryStringInvalidVotingMethod)
            .UseSecretApiKey();

        const string expectedInstance = "GET " + GetCompetingCountryPointsInRangeRankingsRoute + queryStringInvalidVotingMethod;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.BadRequest);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status400BadRequest)
            .And.HasTitle("Bad HTTP request")
            .And.HasDetail("InvalidEnumArgumentException was thrown while handling the request. " +
                           "Please use valid string enum values.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.1");
    }

    [Test]
    public async Task Endpoint_should_return_404_with_ProblemDetails_on_non_existent_aggregate_requested()
    {
        // Arrange
        Guid nonExistentCountryId = Guid.Parse("33e2987f-195f-4b2f-ab0b-8ee1dba76fe5");

        string getCountryRoute = $"/admin/api/v0.2/countries/{nonExistentCountryId}";

        RestRequest request = new RestRequest(getCountryRoute).UseSecretApiKey();

        string expectedInstance = "GET " + getCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.NotFound);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status404NotFound)
            .And.HasTitle("Country not found")
            .And.HasDetail("No country exists with the provided country ID.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.5")
            .And.HasExtension("countryId", nonExistentCountryId);
    }

    [Test]
    public async Task Endpoint_should_return_409_with_ProblemDetails_on_request_conflicting_with_system_state()
    {
        // Arrange
        const string nonUniqueCountryCode = "XX";

        CreateCountryRequest requestBody = new()
        {
            CountryCode = nonUniqueCountryCode, CountryName = "CountryName", CountryType = CountryType.Real
        };

        RestRequest request = new RestRequest(CreateCountryRoute, Method.Post).AddJsonBody(requestBody).UseSecretApiKey();

        const string expectedInstance = "POST " + CreateCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Conflict);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status409Conflict)
            .And.HasTitle("Country code conflict")
            .And.HasDetail("A country already exists with the provided country code.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.10")
            .And.HasExtension("countryCode", nonUniqueCountryCode);
    }

    [Test]
    public async Task Endpoint_should_return_422_with_ProblemDetails_on_intrinsically_illegal_request()
    {
        // Arrange
        const string illegalCountryCode = "!";

        CreateCountryRequest requestBody = new()
        {
            CountryCode = illegalCountryCode, CountryName = "CountryName", CountryType = CountryType.Real
        };

        RestRequest request = new RestRequest(CreateCountryRoute, Method.Post).AddJsonBody(requestBody).UseSecretApiKey();

        const string expectedInstance = "POST " + CreateCountryRoute;

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status422UnprocessableEntity)
            .And.HasTitle("Illegal country code value")
            .And.HasDetail("Country code value must be a string of 2 upper-case letters.")
            .And.HasInstance(expectedInstance)
            .And.HasType("https://tools.ietf.org/html/rfc4918#section-11.2")
            .And.HasExtension("countryCode", illegalCountryCode);
    }
}
