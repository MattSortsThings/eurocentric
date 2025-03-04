using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Eurocentric.WebApp.Tests.Acceptance.Utils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.ErrorHandling;

public static class GlobalExceptionHandlingTests
{
    public sealed class Api(CleanWebAppFixture fixture) : AcceptanceTest(fixture)
    {
        [Fact]
        public async Task Should_return_400_with_ProblemDetails_given_request_body_with_missing_required_property()
        {
            // Arrange
            const string resource = "/admin/api/v0.2/calculations";

            const string missingYPropertyJson = """{ "x": 1, "operation": "Product" }""";

            RestRequest request = Post(resource)
                .UseAdminApiKey()
                .AddJsonBody(missingYPropertyJson);

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemDetails.ShouldHaveTitle("Bad HTTP request"),
                () => problemDetails.ShouldHaveDetail("Failed to read parameter \"CreateCalculationCommand command\" " +
                                                      "from the request body as JSON."),
                () => problemDetails.ShouldHaveInstance("POST " + resource)
            );
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_given_request_body_with_unparseable_enum_property()
        {
            // Arrange
            const string resource = "/admin/api/v0.2/calculations";

            const string invalidOperationPropertyJson = """{ "x": 1, "y": 1, "operation": "NOT_AN_OPERATION_ENUM_VALUE" }""";

            RestRequest request = Post(resource)
                .UseAdminApiKey()
                .AddJsonBody(invalidOperationPropertyJson);

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemDetails.ShouldHaveTitle("Bad HTTP request"),
                () => problemDetails.ShouldHaveDetail("Failed to read parameter \"CreateCalculationCommand command\" " +
                                                      "from the request body as JSON."),
                () => problemDetails.ShouldHaveInstance("POST " + resource)
            );
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_given_query_string_with_missing_required_parameter()
        {
            // Arrange
            const string missingLineParameterResource = "/public/api/v0.1/stations";

            RestRequest request = Get(missingLineParameterResource)
                .UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemDetails.ShouldHaveTitle("Bad HTTP request"),
                () => problemDetails.ShouldHaveDetail("Required parameter \"Line Line\" was not provided from query string."),
                () => problemDetails.ShouldHaveInstance("GET " + missingLineParameterResource)
            );
        }

        [Fact]
        public async Task Should_return_400_with_ProblemDetails_given_query_string_with_unparseable_enum_parameter()
        {
            // Arrange
            const string invalidLineParameterResource = "/public/api/v0.1/stations?Line=NOT_A_LINE_ENUM_VALUE";

            RestRequest request = Get(invalidLineParameterResource)
                .UsePublicApiKey();

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.BadRequest),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemDetails.ShouldHaveTitle("Bad HTTP request"),
                () => problemDetails.ShouldHaveDetail("Failed to bind parameter \"Line Line\" from \"NOT_A_LINE_ENUM_VALUE\"."),
                () => problemDetails.ShouldHaveInstance("GET " + invalidLineParameterResource)
            );
        }

        [Fact]
        public async Task Should_return_500_with_ProblemDetails_when_request_is_well_formed_but_exception_is_thrown()
        {
            // Arrange
            const string resource = "/admin/api/v0.2/calculations";

            CreateCalculationCommand divideByZeroCommand = new() { X = 1, Y = 0, Operation = Operation.Modulus };

            RestRequest request = Post(resource)
                .UseAdminApiKey()
                .AddJsonBody(divideByZeroCommand);

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => statusCode.ShouldBe(HttpStatusCode.InternalServerError),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status500InternalServerError),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.6.1"),
                () => problemDetails.ShouldHaveTitle("Internal server error"),
                () => problemDetails.ShouldHaveDetail("DivideByZeroException was thrown while handling the request."),
                () => problemDetails.ShouldHaveInstance("POST " + resource)
            );
        }
    }
}
