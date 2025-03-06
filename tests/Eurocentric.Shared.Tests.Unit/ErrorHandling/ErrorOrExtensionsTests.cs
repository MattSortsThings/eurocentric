using ErrorOr;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Tests.Unit.Utils;
using Eurocentric.Tests.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Shared.Tests.Unit.ErrorHandling;

public static class ErrorOrExtensionsTests
{
    public sealed class ToHttpResultExtensionMethod : UnitTest
    {
        private const string ErrorCode = "Error";
        private const string ErrorDescription = "An error occurred.";
        private const string TestValue = "A squid eating dough on a polyethylene bag is fast and bulbous. Got me?";

        [Fact]
        public void Should_invoke_mapper_and_return_its_result_when_instance_is_not_error()
        {
            // Arrange
            ErrorOr<string> sut = TestValue.ToErrorOr();

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            Ok<string> okResult = (Ok<string>)result.Result;

            Assert.Equal(TestValue, okResult.Value);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_422_when_instance_first_error_has_Failure_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Failure(ErrorCode, ErrorDescription,
                new Dictionary<string, object> { ["value"] = TestValue });

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status422UnprocessableEntity),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status422UnprocessableEntity),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.21"),
                () => problemResult.ProblemDetails.ShouldHaveExtension("value", TestValue)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_500_when_instance_first_error_has_Unexpected_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Unexpected(ErrorCode, ErrorDescription);

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status500InternalServerError),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status500InternalServerError),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.6.1")
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_400_when_instance_first_error_has_Validation_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Validation(ErrorCode, ErrorDescription,
                new Dictionary<string, object> { ["value"] = TestValue });

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status400BadRequest),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status400BadRequest),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.1"),
                () => problemResult.ProblemDetails.ShouldHaveExtension("value", TestValue)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_409_when_instance_first_error_has_Conflict_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Conflict(ErrorCode, ErrorDescription,
                new Dictionary<string, object> { ["value"] = TestValue });

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status409Conflict),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status409Conflict),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.10"),
                () => problemResult.ProblemDetails.ShouldHaveExtension("value", TestValue)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_404_when_instance_first_error_has_NotFound_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.NotFound(ErrorCode, ErrorDescription,
                new Dictionary<string, object> { ["value"] = TestValue });

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status404NotFound),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status404NotFound),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.5"),
                () => problemResult.ProblemDetails.ShouldHaveExtension("value", TestValue)
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_401_when_instance_first_error_has_Unauthorized_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Unauthorized(ErrorCode, ErrorDescription);

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status401Unauthorized),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status401Unauthorized),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.2")
            );
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_403_when_instance_first_error_has_Forbidden_error_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Forbidden(ErrorCode, ErrorDescription);

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => problemResult.ShouldHaveStatusCode(StatusCodes.Status403Forbidden),
                () => problemResult.ProblemDetails.ShouldHaveTitle(ErrorCode),
                () => problemResult.ProblemDetails.ShouldHaveDetail(ErrorDescription),
                () => problemResult.ProblemDetails.ShouldHaveStatus(StatusCodes.Status403Forbidden),
                () => problemResult.ProblemDetails.ShouldHaveType("https://tools.ietf.org/html/rfc9110#section-15.5.4")
            );
        }
    }
}
