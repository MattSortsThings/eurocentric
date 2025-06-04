using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.UnitTests.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Features.UnitTests.Shared.ErrorHandling;

public sealed class ErrorExtensionsTests : UnitTestBase
{
    private const string Value = "Cha Cha Cha";

    public sealed class ToProblemHttpResultExtensionMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_422_when_instance_ErrorType_is_Failure()
        {
            // Arrange
            Error sut = Error.Failure("Illegal value",
                "Value is illegal.",
                new Dictionary<string, object> { ["value"] = Value });

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(422, result.StatusCode);
            Assert.Equal(422, result.ProblemDetails.Status);
            Assert.Equal("Illegal value", result.ProblemDetails.Title);
            Assert.Equal("Value is illegal.", result.ProblemDetails.Detail);
            Assert.Contains(result.ProblemDetails.Extensions, kvp => kvp is { Key: "value", Value: Value });
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_500_when_instance_ErrorType_is_Unexpected()
        {
            // Arrange
            Error sut = Error.Unexpected("Unexpected error", "Something went wrong.");

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(500, result.StatusCode);
            Assert.Equal(500, result.ProblemDetails.Status);
            Assert.Equal("Unexpected error", result.ProblemDetails.Title);
            Assert.Equal("Something went wrong.", result.ProblemDetails.Detail);
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_400_when_instance_ErrorType_is_Validation()
        {
            // Arrange
            Error sut = Error.Validation("Invalid value",
                "Value is invalid.",
                new Dictionary<string, object> { ["value"] = Value });

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(400, result.StatusCode);
            Assert.Equal(400, result.ProblemDetails.Status);
            Assert.Equal("Invalid value", result.ProblemDetails.Title);
            Assert.Equal("Value is invalid.", result.ProblemDetails.Detail);
            Assert.Contains(result.ProblemDetails.Extensions, kvp => kvp is { Key: "value", Value: Value });
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_409_when_instance_ErrorType_is_Conflict()
        {
            // Arrange
            Error sut = Error.Conflict("Duplicate value",
                "Value is not unique.",
                new Dictionary<string, object> { ["value"] = Value });

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(409, result.StatusCode);
            Assert.Equal(409, result.ProblemDetails.Status);
            Assert.Equal("Duplicate value", result.ProblemDetails.Title);
            Assert.Equal("Value is not unique.", result.ProblemDetails.Detail);
            Assert.Contains(result.ProblemDetails.Extensions, kvp => kvp is { Key: "value", Value: Value });
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_404_when_instance_ErrorType_is_NotFound()
        {
            // Arrange
            Error sut = Error.NotFound("Value not found",
                "Value does not exist.",
                new Dictionary<string, object> { ["value"] = Value });

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(404, result.StatusCode);
            Assert.Equal(404, result.ProblemDetails.Status);
            Assert.Equal("Value not found", result.ProblemDetails.Title);
            Assert.Equal("Value does not exist.", result.ProblemDetails.Detail);
            Assert.Contains(result.ProblemDetails.Extensions, kvp => kvp is { Key: "value", Value: Value });
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_401_when_instance_ErrorType_is_Unauthorized()
        {
            // Arrange
            Error sut = Error.Unauthorized("Unauthorized", "Client is not authenticated.");

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(401, result.StatusCode);
            Assert.Equal(401, result.ProblemDetails.Status);
            Assert.Equal("Unauthorized", result.ProblemDetails.Title);
            Assert.Equal("Client is not authenticated.", result.ProblemDetails.Detail);
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }

        [Fact]
        public void Should_return_ProblemHttpResult_with_status_code_403_when_instance_ErrorType_is_Forbidden()
        {
            // Arrange
            Error sut = Error.Forbidden("Forbidden", "Client is not authorized.");

            // Act
            ProblemHttpResult result = sut.ToProblemHttpResult();

            // Assert
            Assert.Equal(403, result.StatusCode);
            Assert.Equal(403, result.ProblemDetails.Status);
            Assert.Equal("Forbidden", result.ProblemDetails.Title);
            Assert.Equal("Client is not authorized.", result.ProblemDetails.Detail);
            Assert.NotNull(result.ProblemDetails.Type);
            Assert.Null(result.ProblemDetails.Instance);
        }
    }
}
