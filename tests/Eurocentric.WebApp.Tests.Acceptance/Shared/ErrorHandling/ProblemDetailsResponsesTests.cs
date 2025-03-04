using System.Net;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.ErrorHandling;

public static class ProblemDetailsResponsesTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_unsuccessful_response_with_ProblemDetails_given_valid_but_unsuccessful_request()
        {
            // Arrange
            Guid calculationId = Guid.Empty;

            string resource = "/admin/api/v0.1/calculations/" + calculationId;

            RestRequest request = Get(resource).UseAdminApiKey();

            // Assert
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.NotFound),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status404NotFound),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.5"),
                () => problemDetails.ShouldHaveTitle("Calculation not found"),
                () => problemDetails.ShouldHaveDetail("No calculation exists with the specified ID."),
                () => problemDetails.ShouldHaveInstance("GET " + resource),
                () => problemDetails.ShouldHaveExtension("calculationId", calculationId)
            );
        }
    }
}
