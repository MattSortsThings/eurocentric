using System.Net;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Eurocentric.TestUtils.WebAppFixtures;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.PublicApi.V0.Greetings;

public static class GetGreetingsTests
{
    public sealed class Endpoint : AcceptanceTest
    {
        private const string Resource = "public/api/v0.1/greetings";

        public Endpoint(WebAppFixture webAppFixture) : base(webAppFixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_response_given_valid_request()
        {
            // Arrange
            RestRequest request = new RestRequest(Resource)
                .AddQueryParameter("quantity", 2)
                .AddQueryParameter("language", "Dutch")
                .AddHeader("Accept", "application/json");

            // Act
            RestResponse<GetGreetingsResponse> response =
                await Sut.ExecuteAsync<GetGreetingsResponse>(request, TestContext.Current.CancellationToken);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.OK, response.StatusCode),
                () => Assert.Equal(2, response.Data!.Greetings.Length)
            );
        }
    }
}
