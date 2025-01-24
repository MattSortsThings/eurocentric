using ErrorOr;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Tests.Unit.ErrorHandling.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Shared.Tests.Unit.ErrorHandling;

public static class ErrorOrExtensionsTests
{
    public sealed class MapToResultsExtensionMethod
    {
        private const int Value = 999;

        [Fact]
        public void Should_return_mapped_result_given_instance_is_not_error()
        {
            // Arrange
            ErrorOr<int> sut = ErrorOrFactory.From(Value);

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            Ok<int> okResult = (Ok<int>)result.Result;

            Assert.Equal(Value, okResult.Value);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_422_given_instance_first_error_is_Failure_type()
        {
            // Arrange
            const string errorCode = "Unprocessable";
            const string errorDescription = "Could not process request";

            ErrorOr<int> sut = Error.Failure(errorCode, errorDescription);

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status422UnprocessableEntity),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status422UnprocessableEntity),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc4918#section-11.2"),
                () => problemDetails.ShouldHaveEmptyExtensions()
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_500_given_instance_first_error_is_Unexpected_type()
        {
            // Arrange
            const string errorCode = "UnexpectedError";
            const string errorDescription = "Something went wrong";

            ErrorOr<int> sut = Error.Unexpected(errorCode, errorDescription);

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status500InternalServerError),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status500InternalServerError),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.6.1"),
                () => problemDetails.ShouldHaveEmptyExtensions()
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_400_given_instance_first_error_is_Validation_type()
        {
            // Arrange
            const string errorCode = "InvalidValue";
            const string errorDescription = "Value cannot be greater than 100.";

            ErrorOr<int> sut = Error.Validation(errorCode, errorDescription,
                new Dictionary<string, object> { { "value", Value } });

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemDetails.ShouldHaveExtensionsEntry("value", Value)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_409_given_instance_first_error_is_Conflict_type()
        {
            // Arrange
            const string errorCode = "ValueConflict";
            const string errorDescription = "Value already exists.";

            ErrorOr<int> sut =
                Error.Conflict(errorCode, errorDescription, new Dictionary<string, object> { { "value", Value } });

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status409Conflict),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status409Conflict),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.10"),
                () => problemDetails.ShouldHaveExtensionsEntry("value", Value)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_404_given_instance_first_error_is_NotFound_type()
        {
            // Arrange
            const string errorCode = "ValueNotFound";
            const string errorDescription = "Value does not exist.";

            ErrorOr<int> sut =
                Error.NotFound(errorCode, errorDescription, new Dictionary<string, object> { { "value", Value } });

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status404NotFound),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status404NotFound),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.5"),
                () => problemDetails.ShouldHaveExtensionsEntry("value", Value)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_401_given_instance_first_error_is_Unauthorized_type()
        {
            // Arrange
            const string errorCode = "Unauthorized";
            const string errorDescription = "Client is not authenticated.";

            ErrorOr<int> sut = Error.Unauthorized(errorCode, errorDescription);

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status401Unauthorized),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status401Unauthorized),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.2"),
                () => problemDetails.ShouldHaveEmptyExtensions()
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_403_given_instance_first_error_is_Forbidden_type()
        {
            // Arrange
            const string errorCode = "Forbidden";
            const string errorDescription = "Client is not authorized.";

            ErrorOr<int> sut = Error.Forbidden(errorCode, errorDescription);

            // Act
            Results<Ok<int>, ProblemHttpResult> result = sut.MapToResults(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;
            ProblemDetails problemDetails = problemResult.ProblemDetails;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status403Forbidden),
                () => problemDetails.ShouldHaveTitle(errorCode),
                () => problemDetails.ShouldHaveDetail(errorDescription),
                () => problemDetails.ShouldHaveStatus(StatusCodes.Status403Forbidden),
                () => problemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.4"),
                () => problemDetails.ShouldHaveEmptyExtensions()
            );
        }

        [Fact]
        public void Should_throw_exception_given_mapper_arg_is_null()
        {
            // Arrange
            ErrorOr<int> sut = ErrorOrFactory.From(Value);

            // Act
            Exception? exception = Record.Exception(() => sut.MapToResults<int, Ok>(null!));

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(exception),
                () => Assert.IsType<ArgumentNullException>(exception),
                () => Assert.Equal("Value cannot be null. (Parameter 'mapper')", exception!.Message)
            );
        }
    }
}
