using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class PublicApiV0ApiKeySecurityTests : SeededParallelAcceptanceTest
{
    private const string GetQueryableVotingMethodsRoute = "/public/api/v0.2/queryables/voting-methods";

    [Test]
    public async Task PublicApi_V0_endpoint_should_authenticate_and_authorize_client_using_secret_API_key()
    {
        // Arrange
        RestRequest request = GetRequest(GetQueryableVotingMethodsRoute).AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.IsSuccessful).IsTrue();
    }

    [Test]
    public async Task PublicApi_V0_endpoint_should_authenticate_and_authorize_client_using_demo_API_key()
    {
        // Arrange
        RestRequest request = GetRequest(GetQueryableVotingMethodsRoute).AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.IsSuccessful).IsTrue();
    }

    [Test]
    public async Task PublicApi_V0_endpoint_should_not_authenticate_client_using_unrecognized_API_key()
    {
        // Arrange
        RestRequest request = GetRequest(GetQueryableVotingMethodsRoute).AddHeader("X-Api-Key", "NOT_AN_API_KEY");

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        await Assert.That(problemResponse.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task PublicApi_V0_endpoint_should_not_authenticate_client_using_no_API_key()
    {
        // Arrange
        RestRequest request = GetRequest(GetQueryableVotingMethodsRoute);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        await Assert.That(problemResponse.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }
}
