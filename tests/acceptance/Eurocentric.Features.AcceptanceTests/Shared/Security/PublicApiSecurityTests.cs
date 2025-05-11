using System.Net;
using Eurocentric.Features.AcceptanceTests.Shared.TestUtils;
using Eurocentric.Features.AcceptanceTests.TestUtils;
using RestSharp;

namespace Eurocentric.Features.AcceptanceTests.Shared.Security;

public sealed class PublicApiSecurityTests : AcceptanceTestBase
{
    private const string Route = "public/api/v0.1/filters/voting-methods";

    public PublicApiSecurityTests(WebAppFixture fixture) : base(fixture) { }

    [Fact]
    public async Task Should_authenticate_and_authorize_request_with_X_Api_Key_request_header_containing_secret_API_key()
    {
        // Arrange
        RestRequest request = new(Route);

        request.AddHeader("X-Api-Key", TestApiKeys.Secret);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = responseOrProblem.AsT0.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }

    [Fact]
    public async Task Should_authenticate_and_authorize_request_with_X_Api_Key_request_header_containing_demo_API_key()
    {
        // Arrange
        RestRequest request = new(Route);

        request.AddHeader("X-Api-Key", TestApiKeys.Demo);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = responseOrProblem.AsT0.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }

    [Fact]
    public async Task Should_not_authenticate_request_with_X_Api_Key_request_header_containing_unrecognized_API_key()
    {
        // Arrange
        RestRequest request = new(Route);

        request.AddHeader("X-Api-Key", TestApiKeys.Unrecognized);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = responseOrProblem.AsT1.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
    }

    [Fact]
    public async Task Should_not_authenticate_request_with_no_X_Api_Key_request()
    {
        // Arrange
        RestRequest request = new(Route);

        // Act
        ResponseOrProblem responseOrProblem = await Client.SendRequestAsync(request, TestContext.Current.CancellationToken);

        HttpStatusCode statusCode = responseOrProblem.AsT1.StatusCode;

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
    }
}
