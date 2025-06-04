using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.Utilities;
using Eurocentric.Features.AcceptanceTests.Utilities;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class AdminApiSecurityTests(WebAppFixture fixture) : AcceptanceTestBase(fixture)
{
    private const string GetContestsRoute = "/admin/api/v0.2/contests";

    [Fact]
    public async Task Should_be_authenticated_and_authorized_given_API_key_request_header_containing_secret_API_key()
    {
        // Arrange
        RestRequest adminApiRequest = new(GetContestsRoute);

        adminApiRequest.AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(adminApiRequest, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = problemOrResponse.AsResponse.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }

    [Fact]
    public async Task Should_be_authenticated_but_not_authorized_given_API_key_request_header_containing_demo_API_key()
    {
        // Arrange
        RestRequest adminApiRequest = new(GetContestsRoute);

        adminApiRequest.AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(adminApiRequest, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, statusCode);
    }

    [Fact]
    public async Task Should_not_be_authenticated_given_API_key_request_header_containing_unrecognized_API_key()
    {
        // Arrange
        RestRequest adminApiRequest = new(GetContestsRoute);

        adminApiRequest.AddHeader("X-Api-Key", "THIS_IS_NOT_A_KEY");

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(adminApiRequest, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
    }

    [Fact]
    public async Task Should_not_be_authenticated_given_no_API_key_request_header()
    {
        // Arrange
        RestRequest adminApiRequest = new(GetContestsRoute);

        // Act
        ProblemOrResponse problemOrResponse =
            await SutRestClient.SendRequestAsync(adminApiRequest, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = problemOrResponse.AsProblem.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
    }
}
