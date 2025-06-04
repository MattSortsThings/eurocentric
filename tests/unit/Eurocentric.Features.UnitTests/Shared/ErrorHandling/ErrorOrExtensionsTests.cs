using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.UnitTests.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Features.UnitTests.Shared.ErrorHandling;

public sealed class ErrorOrExtensionsTests : UnitTestBase
{
    private const int Value = 999;

    public sealed class ToProblemOrResponseAsyncExtensionMethod : UnitTestBase
    {
        [Fact]
        public async Task Should_return_result_of_mappingFunction_arg_when_instance_is_not_response()
        {
            // Arrange
            ErrorOr<int> sut = ErrorOrFactory.From(Value);
            Func<int, Ok<int>> mappingFunction = TypedResults.Ok;

            // Act
            Results<ProblemHttpResult, Ok<int>> result =
                await Task.FromResult(sut).ToProblemOrResponseAsync(mappingFunction);

            // Assert
            Ok<int> okResult = Assert.IsType<Ok<int>>(result.Result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(Value, okResult.Value);
        }

        [Fact]
        public async Task Should_return_ProblemHttpResult_mapped_from_first_error_when_instance_is_errors()
        {
            // Arrange
            ErrorOr<int> sut = Error.Conflict("Duplicate value",
                "Value is not unique",
                new Dictionary<string, object> { ["value"] = Value });

            Func<int, Ok<int>> dummyMappingFunction = TypedResults.Ok;

            // Act
            Results<ProblemHttpResult, Ok<int>> result =
                await Task.FromResult(sut).ToProblemOrResponseAsync(dummyMappingFunction);

            // Assert
            ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

            Assert.Equal(StatusCodes.Status409Conflict, problemResult.StatusCode);

            Assert.Equal(StatusCodes.Status409Conflict, problemResult.ProblemDetails.Status);
            Assert.Equal("Duplicate value", problemResult.ProblemDetails.Title);
            Assert.Equal("Value is not unique", problemResult.ProblemDetails.Detail);
            Assert.Contains(problemResult.ProblemDetails.Extensions, kvp => kvp is { Key: "value", Value: Value });
        }
    }
}
