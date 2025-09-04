using System.Net;
using System.Text.Json;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using TUnit.Assertions.Enums;

namespace Eurocentric.Features.AcceptanceTests.Shared.Documentation;

public sealed class OpenApiEndpointTests : ParallelSeededAcceptanceTest
{
    [Test]
    [Arguments("admin-api-v0.1")]
    [Arguments("admin-api-v0.2")]
    [Arguments("admin-api-v1.0")]
    [Arguments("public-api-v0.1")]
    [Arguments("public-api-v0.2")]
    public async Task Endpoint_should_retrieve_requested_OpenAPI_document_for_anonymous_client(string docName)
    {
        // Arrange
        RestRequest request = new RestRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        await Assert.That(response.IsSuccessful).IsTrue();
    }

    [Test]
    [Arguments("admin-api-v0.1", "/admin/api/v0.1")]
    [Arguments("admin-api-v0.2", "/admin/api/v0.2")]
    [Arguments("admin-api-v1.0", "/admin/api/v1.0")]
    [Arguments("public-api-v0.1", "/public/api/v0.1")]
    [Arguments("public-api-v0.2", "/public/api/v0.2")]
    public async Task OpenAPI_document_should_have_server_with_expected_url_suffix(
        string docName,
        string expectedServerUrlSuffix)
    {
        // Arrange
        RestRequest request = new RestRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse openApiResponse = await Assert.That(response.AsSuccessful).IsNotNull();

        string serverUrl = await ExtractServerUrlAsync(openApiResponse.Content);

        await Assert.That(serverUrl).EndsWith(expectedServerUrlSuffix);
    }

    [Test]
    [MethodDataSource(typeof(OpenApiEndpointTests), nameof(ExpectedPathsTestData))]
    public async Task OpenAPI_document_should_have_expected_paths(string docName, string[] expectedPaths)
    {
        // Arrange
        RestRequest request = new RestRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse openApiResponse = await Assert.That(response.AsSuccessful).IsNotNull();

        string[] actualPaths = await ExtractAllPathsAsync(openApiResponse.Content);

        await Assert.That(actualPaths).IsEquivalentTo(expectedPaths, CollectionOrdering.Any);
    }

    [Test]
    [Arguments("admin-api-v0")]
    [Arguments("admin-api-v0.3")]
    [Arguments("admin-api-v1")]
    [Arguments("admin-api-v1.1")]
    [Arguments("public-api-v0")]
    [Arguments("public-api-v0.3")]
    [Arguments("public-api-v1.0")]
    public async Task Endpoint_should_fail_on_non_existent_OpenAPI_document_requested(string docName)
    {
        // Arrange
        RestRequest request = new RestRequest("/openapi/{docName}.json").AddUrlSegment("docName", docName);

        // Act
        BiRestResponse response = await SystemUnderTest.SendAsync(request, TestContext.Current!.CancellationToken);

        // Assert
        RestResponse<ProblemDetails> problemResponse = await Assert.That(response.AsUnsuccessful).IsNotNull();

        await Assert.That(problemResponse.StatusCode).IsEqualTo(HttpStatusCode.NotFound);
    }

    public static IEnumerable<Func<(string, string[])>> ExpectedPathsTestData()
    {
        yield return () => ("admin-api-v0.1", [
            "/countries",
            "/countries/{countryId}"
        ]);

        yield return () => ("admin-api-v0.2", [
            "/countries",
            "/countries/{countryId}"
        ]);

        yield return () => ("admin-api-v1.0", [
            "/countries/{countryId}"
        ]);

        yield return () => ("public-api-v0.1", [
            "/queryables/contest-stages",
            "/queryables/countries",
            "/queryables/voting-methods"
        ]);

        yield return () => ("public-api-v0.2", [
            "/queryables/contest-stages",
            "/queryables/countries",
            "/queryables/voting-methods",
            "/rankings/competing-countries/points-in-range"
        ]);
    }

    private static async Task<string[]> ExtractAllPathsAsync(string? json)
    {
        string jsonText = await Assert.That(json).IsNotNull();

        using JsonDocument doc = JsonDocument.Parse(jsonText);

        return doc.RootElement.GetProperty("paths")
            .EnumerateObject()
            .Select(property => property.Name)
            .ToArray();
    }

    private static async Task<string> ExtractServerUrlAsync(string? json)
    {
        string jsonText = await Assert.That(json).IsNotNull();

        using JsonDocument doc = JsonDocument.Parse(jsonText);

        JsonElement server = doc.RootElement.GetProperty("servers")
            .EnumerateArray()
            .Single();

        string? serverUrl = server.GetProperty("url").GetString();

        return await Assert.That(serverUrl).IsNotNull();
    }
}
