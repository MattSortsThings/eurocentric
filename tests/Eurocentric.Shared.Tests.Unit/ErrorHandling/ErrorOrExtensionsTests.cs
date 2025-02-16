using ErrorOr;
using Eurocentric.Shared.ErrorHandling;
using Eurocentric.Shared.Tests.Unit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Shared.Tests.Unit.ErrorHandling;

public static class ErrorOrExtensionsTests
{
    public sealed class ToHttpResultExtensionMethod : UnitTest
    {
        private const string Value = "Cha Cha Cha";

        [Fact]
        public void Should_return_happy_path_mapper_result_when_instance_is_not_error()
        {
            // Arrange
            ErrorOr<string> sut = Value.ToErrorOr();

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            Ok<string> okResult = (Ok<string>)result.Result;

            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode),
                () => Assert.Equal(Value, okResult.Value)
            );
        }

        [Fact]
        public void Should_return_problem_result_with_status_code_422_when_instance_is_error_with_failure_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Failure("Invalid value",
                "Value is invalid",
                new Dictionary<string, object> { ["value"] = Value });

            object expectedProblemDetails = new
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Invalid value",
                Detail = "Value is invalid",
                Extensions = new Dictionary<string, object> { ["value"] = Value }
            };

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemResult.StatusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemResult.ProblemDetails)
            );
        }

        [Fact]
        public void Should_return_problem_result_with_status_code_409_when_instance_is_error_with_conflict_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Conflict("Value conflict",
                "Value is not unique",
                new Dictionary<string, object> { ["value"] = Value });

            object expectedProblemDetails = new
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Value conflict",
                Detail = "Value is not unique",
                Extensions = new Dictionary<string, object> { ["value"] = Value }
            };

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status409Conflict, problemResult.StatusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemResult.ProblemDetails)
            );
        }


        [Fact]
        public void Should_return_problem_result_with_status_code_404_when_instance_is_error_with_not_found_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.NotFound("Value not found",
                "Value was not found",
                new Dictionary<string, object> { ["value"] = Value });

            object expectedProblemDetails = new
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Value not found",
                Detail = "Value was not found",
                Extensions = new Dictionary<string, object> { ["value"] = Value }
            };

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status404NotFound, problemResult.StatusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemResult.ProblemDetails)
            );
        }

        [Fact]
        public void Should_return_problem_result_with_status_code_400_when_instance_is_error_with_validation_type()
        {
            // Arrange
            ErrorOr<string> sut = Error.Validation("Invalid value",
                "Value is invalid",
                new Dictionary<string, object> { ["value"] = Value });

            object expectedProblemDetails = new
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid value",
                Detail = "Value is invalid",
                Extensions = new Dictionary<string, object> { ["value"] = Value }
            };

            // Act
            Results<Ok<string>, ProblemHttpResult> result = sut.ToHttpResult(TypedResults.Ok);

            // Assert
            ProblemHttpResult problemResult = (ProblemHttpResult)result.Result;

            Assert.Multiple(
                () => Assert.Equal(StatusCodes.Status400BadRequest, problemResult.StatusCode),
                () => Assert.Equivalent(expectedProblemDetails, problemResult.ProblemDetails)
            );
        }
    }
}
