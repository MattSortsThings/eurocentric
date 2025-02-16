using System.Net;
using Eurocentric.PublicApi.V0.Greetings.GetGreetings;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.PublicApi.V0.Greetings;

public static class GetGreetingsTests
{
    public sealed class Feature : AcceptanceTest
    {
        public Feature(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_200_with_greetings_given_valid_request()
        {
            // Arrange
            RestRequest request = Get("public/api/v0.1/greetings")
                .AddQueryParameter("quantity", "2")
                .AddQueryParameter("language", "Dutch")
                .AddQueryParameter("client_name", "Conchita");

            // Act
            (HttpStatusCode statusCode, GetGreetingsResult result) = await SendAsync<GetGreetingsResult>(request);

            // Assert
            GetGreetingsResult expected = new(["Hoi Conchita!", "Hoi Conchita!"]);

            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.OK, statusCode),
                () => Assert.Equivalent(expected, result)
            );
        }
    }
}
