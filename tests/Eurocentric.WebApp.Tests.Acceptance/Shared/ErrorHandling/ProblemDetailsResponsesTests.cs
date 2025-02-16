using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.ErrorHandling;

public static class ProblemDetailsResponsesTests
{
    public sealed class Api : AcceptanceTest
    {
        public Api(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_422_with_problem_details_when_request_is_unsuccessful()
        {
            // Arrange
            RestRequest request = Get("public/api/v0.1/greetings?quantity=0&language=English");

            object expectedProblemDetails = new
            {
                Status = 422,
                Title = "Invalid greetings quantity",
                Detail = "Greetings quantity cannot be 0.",
                Instance = "GET /public/api/v0.1/greetings?quantity=0&language=English"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.UnprocessableContent, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }
    }
}
