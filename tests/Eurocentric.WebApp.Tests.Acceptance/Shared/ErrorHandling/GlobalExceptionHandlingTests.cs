using System.Net;
using Eurocentric.AdminApi.V0.Calculations.CreateCalculation;
using Eurocentric.AdminApi.V0.Calculations.Models;
using Eurocentric.WebApp.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Eurocentric.WebApp.Tests.Acceptance.Shared.ErrorHandling;

public static class GlobalExceptionHandlingTests
{
    public sealed class Api : AcceptanceTest
    {
        public Api(CleanWebAppFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_request_body_with_missing_required_property()
        {
            // Arrange
            const string jsonMissingYProperty = """
                                                {
                                                   "x": 1,
                                                   "operation": "Product"
                                                }
                                                """;

            RestRequest request = Post("admin/api/v0.1/calculations")
                .AddJsonBody(jsonMissingYProperty);

            object expectedProblemDetails = new
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Bad HTTP request",
                Detail = "Failed to read parameter \"CreateCalculationCommand command\" from the request body as JSON.",
                Instance = "POST /admin/api/v0.1/calculations"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_request_body_with_unparseable_enum_property()
        {
            // Arrange
            const string jsonUnparseableOperationProperty = """
                                                            {
                                                               "x": 1,
                                                               "y": 1,
                                                               "operation": "NOT_AN_OPERATION_ENUM_VALUE"
                                                            }
                                                            """;

            RestRequest request = Post("admin/api/v0.1/calculations")
                .AddJsonBody(jsonUnparseableOperationProperty);

            object expectedProblemDetails = new
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Bad HTTP request",
                Detail = "Failed to read parameter \"CreateCalculationCommand command\" from the request body as JSON.",
                Instance = "POST /admin/api/v0.1/calculations"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_request_with_missing_required_query_parameter()
        {
            // Arrange
            RestRequest request = Get("public/api/v0.1/greetings")
                .AddQueryParameter("language", "English");

            object expectedProblemDetails = new
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Bad HTTP request",
                Detail = "Required parameter \"int Quantity\" was not provided from query string.",
                Instance = "GET /public/api/v0.1/greetings?language=English"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }

        [Fact]
        public async Task Should_return_400_with_problem_details_given_request_with_unparseable_enum_query_parameter()
        {
            // Arrange
            RestRequest request = Get("public/api/v0.1/greetings")
                .AddQueryParameter("quantity", 1)
                .AddQueryParameter("language", "NOT_A_LANGUAGE_ENUM_VALUE");

            object expectedProblemDetails = new
            {
                Status = HttpStatusCode.BadRequest,
                Title = "Bad HTTP request",
                Detail = "Failed to bind parameter \"Language Language\" from \"NOT_A_LANGUAGE_ENUM_VALUE\".",
                Instance = "GET /public/api/v0.1/greetings?quantity=1&language=NOT_A_LANGUAGE_ENUM_VALUE"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.BadRequest, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }

        [Fact]
        public async Task Should_return_500_with_problem_details_when_exception_is_thrown()
        {
            // Arrange
            RestRequest request = Post("admin/api/v0.1/calculations")
                .AddJsonBody(new CreateCalculationCommand { X = 1, Y = 0, Operation = Operation.Modulus });

            object expectedProblemDetails = new
            {
                Status = HttpStatusCode.InternalServerError,
                Title = "Internal server error",
                Detail = "DivideByZeroException was thrown while handling the request.",
                Instance = "POST /admin/api/v0.1/calculations"
            };

            // Act
            (HttpStatusCode statusCode, ProblemDetails problemDetails) = await SendAsync<ProblemDetails>(request);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(HttpStatusCode.InternalServerError, statusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemDetails)
            );
        }
    }
}
