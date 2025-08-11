using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utils;
using Eurocentric.Features.AcceptanceTests.Utils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class AdminApiV1SecurityTests : ParallelCleanAcceptanceTest
{
    private const string GetCountriesRoute = "/admin/api/v1.0/countries";

    [Test]
    public async Task AdminApi_V1_endpoint_should_authenticate_and_authorize_request_using_secret_API_key()
    {
        // Arrange
        RestRequest request = new RestRequest(GetCountriesRoute)
            .AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        // Assert
        HttpStatusCode statusCode = result.AsResponse.StatusCode;

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    public async Task AdminApi_V1_endpoint_should_authenticate_but_not_authorize_request_using_demo_API_key()
    {
        // Arrange
        RestRequest request = new RestRequest(GetCountriesRoute)
            .AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        // Assert
        HttpStatusCode statusCode = result.AsProblem.StatusCode;

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Forbidden);
    }

    [Test]
    public async Task AdminApi_V1_endpoint_should_not_authenticate_request_using_unrecognized_API_key()
    {
        // Arrange
        RestRequest request = new RestRequest(GetCountriesRoute)
            .AddHeader("X-Api-Key", "CECI_N'EST_PAS_UNE_CLE");

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        // Assert
        HttpStatusCode statusCode = result.AsProblem.StatusCode;

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task AdminApi_V1_endpoint_should_not_authenticate_request_using_no_API_key()
    {
        // Arrange
        RestRequest request = new(GetCountriesRoute);

        // Act
        ProblemOrResponse result = await SystemUnderTest.SendAsync(request);

        // Assert
        HttpStatusCode statusCode = result.AsProblem.StatusCode;

        await Assert.That(statusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }
}
