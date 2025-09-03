using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils.Assertions;
using Eurocentric.Features.AdminApi.V0.Common.Enums;
using Eurocentric.Features.AdminApi.V0.Countries.CreateCountry;
using Eurocentric.Features.AdminApi.V0.Countries.GetCountry;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.ErrorHandling;

public sealed class ErrorHandlingTests : SeededParallelAcceptanceTest
{
    [Test]
    public async Task Endpoint_should_return_404_with_ProblemDetails_on_non_existent_aggregate_requested()
    {
        // Arrange
        Guid nonExistentCountryId = Guid.Parse("33e2987f-195f-4b2f-ab0b-8ee1dba76fe5");

        RestRequest getCountryRequest = GetRequest("/admin/api/v0.1/countries/{countryId}")
            .AddUrlSegment("countryId", nonExistentCountryId);

        // Act
        BiRestResponse<GetCountryResponse> response =
            await SystemUnderTest.SendAsync<GetCountryResponse>(getCountryRequest, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.NotFound);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status404NotFound)
            .And.HasTitle("Country not found")
            .And.HasDetail("No country exists with the provided country ID.")
            .And.HasInstance("GET /admin/api/v0.1/countries/33e2987f-195f-4b2f-ab0b-8ee1dba76fe5")
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.5")
            .And.HasExtension("countryId", nonExistentCountryId);
    }

    [Test]
    public async Task Endpoint_should_return_409_with_ProblemDetails_on_request_conflicting_with_system_state()
    {
        // Arrange
        const string nonUniqueCountryCode = TestConstants.ExistingCountry.CountryCode;

        CreateCountryRequest requestBody = new()
        {
            CountryCode = nonUniqueCountryCode, CountryName = "CountryName", CountryType = CountryType.Real
        };

        RestRequest createCountryRequest = PostRequest("/admin/api/v0.1/countries")
            .AddJsonBody(requestBody);

        // Act
        BiRestResponse<CreateCountryResponse> response =
            await SystemUnderTest.SendAsync<CreateCountryResponse>(createCountryRequest, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Conflict);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status409Conflict)
            .And.HasTitle("Country code conflict")
            .And.HasDetail("A country already exists with the provided country code.")
            .And.HasInstance("POST /admin/api/v0.1/countries")
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

        RestRequest createCountryRequest = PostRequest("/admin/api/v0.1/countries")
            .AddJsonBody(requestBody);

        // Act
        BiRestResponse<CreateCountryResponse> response =
            await SystemUnderTest.SendAsync<CreateCountryResponse>(createCountryRequest, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        (HttpStatusCode statusCode, ProblemDetails? problemDetails) = (problemResponse.StatusCode, problemResponse.Data);

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.UnprocessableEntity);

        await Assert.That(problemDetails).IsNotNull()
            .And.HasStatus(StatusCodes.Status422UnprocessableEntity)
            .And.HasTitle("Illegal country code value")
            .And.HasDetail("Country code value must be a string of 2 upper-case letters.")
            .And.HasInstance("POST /admin/api/v0.1/countries")
            .And.HasType("https://tools.ietf.org/html/rfc4918#section-11.2")
            .And.HasExtension("countryCode", illegalCountryCode);
    }
}
