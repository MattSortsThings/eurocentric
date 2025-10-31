using System.Net;
using Eurocentric.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.AcceptanceTests.NonFunctional.Security;

[Category("security")]
public sealed class ApiKeySecurityTests : ParallelSeededAcceptanceTest
{
    private const string UnrecognizedApiKey = "Ceci_n'est_pas_une_clef";

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Admin_API_v0_x_should_authenticate_client_using_SECRET_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Admin_API_v0_x_should_authenticate_but_not_authorize_client_using_DEMO_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Forbidden);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Admin_API_v0_x_should_not_authenticate_client_using_unrecognized_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", UnrecognizedApiKey);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Admin_API_v0_x_should_not_authenticate_client_using_no_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    [Arguments("v1.0")]
    public async Task Admin_API_v1_x_should_authenticate_client_using_SECRET_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    [Arguments("v1.0")]
    public async Task Admin_API_v1_x_should_authenticate_but_not_authorize_client_using_DEMO_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Forbidden);
    }

    [Test]
    [Arguments("v1.0")]
    public async Task Admin_API_v1_x_should_not_authenticate_client_using_unrecognized_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", UnrecognizedApiKey);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    [Arguments("v1.0")]
    public async Task Admin_API_v1_x_should_not_authenticate_client_using_no_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/admin/api/{apiVersion}/countries")
            .AddUrlSegment("apiVersion", apiVersion);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Public_API_v0_x_should_authenticate_client_using_SECRET_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/public/api/{apiVersion}/queryables/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Public_API_v0_x_should_authenticate_client_using_DEMO_API_KEY(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/public/api/{apiVersion}/queryables/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse response = await Assert.That(problemOrResponse).IsResponse().And.IsNotNull();

        await Assert.That(response.StatusCode).IsEqualTo(HttpStatusCode.OK);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Public_API_v0_x_should_not_authenticate_client_using_unrecognized_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/public/api/{apiVersion}/queryables/countries")
            .AddUrlSegment("apiVersion", apiVersion)
            .AddHeader("X-Api-Key", UnrecognizedApiKey);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    [Test]
    [Arguments("v0.1")]
    [Arguments("v0.2")]
    public async Task Public_API_v0_x_should_not_authenticate_client_using_no_API_key(string apiVersion)
    {
        // Arrange
        RestRequest getCountriesRequest = GetRequest("/public/api/{apiVersion}/queryables/countries")
            .AddUrlSegment("apiVersion", apiVersion);

        // Act
        ProblemOrResponse problemOrResponse = await SystemUnderTest.SendAsync(getCountriesRequest);

        // Assert
        RestResponse<ProblemDetails> problem = await Assert.That(problemOrResponse).IsProblem().And.IsNotNull();

        await Assert.That(problem.StatusCode).IsEqualTo(HttpStatusCode.Unauthorized);
    }

    private static RestRequest GetRequest(string route) => new(route);
}
