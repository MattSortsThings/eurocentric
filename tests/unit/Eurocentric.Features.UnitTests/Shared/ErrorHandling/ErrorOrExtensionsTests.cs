using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.UnitTests.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eurocentric.Features.UnitTests.Shared.ErrorHandling;

public sealed class ErrorOrExtensionsTests : UnitTestBase
{
    private const string Value = "Cha Cha Cha";

    private static readonly Func<string, Ok> DummyMappingFunction = _ => TypedResults.Ok();

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_mapped_response_when_instance_is_not_error()
    {
        // Arrange
        Func<string, Ok<string>> mappingFunction = TypedResults.Ok;

        ErrorOr<string> sut = ErrorOrFactory.From(Value);

        // Act
        Results<Ok<string>, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(mappingFunction);

        // Assert
        Ok<string> okResult = Assert.IsType<Ok<string>>(result.Result);

        Assert.Equal(Value, okResult.Value);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Failure_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Failure("Unprocessable", "Could not process value.");

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status422UnprocessableEntity, statusCode);

        Assert.Equal(StatusCodes.Status422UnprocessableEntity, problemDetails.Status);
        Assert.Equal("Unprocessable", problemDetails.Title);
        Assert.Equal("Could not process value.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.Empty(problemDetails.Extensions);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Unexpected_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Unexpected("Exception", "An exception was thrown.");

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status500InternalServerError, statusCode);

        Assert.Equal(StatusCodes.Status500InternalServerError, problemDetails.Status);
        Assert.Equal("Exception", problemDetails.Title);
        Assert.Equal("An exception was thrown.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.Empty(problemDetails.Extensions);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Validation_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Validation("Invalid",
            "Invalid value.",
            new Dictionary<string, object> { ["value"] = Value });

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status400BadRequest, statusCode);

        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal("Invalid", problemDetails.Title);
        Assert.Equal("Invalid value.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.True(problemDetails.Extensions.TryGetValue("value", out object? v) && v is Value);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Conflict_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Conflict("Value conflict",
            "Value is not unique.",
            new Dictionary<string, object> { ["value"] = Value });

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status409Conflict, statusCode);

        Assert.Equal(StatusCodes.Status409Conflict, problemDetails.Status);
        Assert.Equal("Value conflict", problemDetails.Title);
        Assert.Equal("Value is not unique.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.True(problemDetails.Extensions.TryGetValue("value", out object? v) && v is Value);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_NotFound_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.NotFound("Not found",
            "No entity exists with the specified value.",
            new Dictionary<string, object> { ["value"] = Value });

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status404NotFound, statusCode);

        Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        Assert.Equal("Not found", problemDetails.Title);
        Assert.Equal("No entity exists with the specified value.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.True(problemDetails.Extensions.TryGetValue("value", out object? v) && v is Value);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Unauthorized_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Unauthorized("Unauthorized", "Client not authenticated.");

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status401Unauthorized, statusCode);

        Assert.Equal(StatusCodes.Status401Unauthorized, problemDetails.Status);
        Assert.Equal("Unauthorized", problemDetails.Title);
        Assert.Equal("Client not authenticated.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.Empty(problemDetails.Extensions);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_return_ProblemHttpResult_when_instance_first_error_has_Forbidden_type()
    {
        // Arrange
        ErrorOr<string> sut = Error.Forbidden("Forbidden", "Client not authorized.");

        // Act
        Results<Ok, ProblemHttpResult> result = await Task.FromResult(sut).ToResultOrProblemAsync(DummyMappingFunction);

        // Assert
        ProblemHttpResult problemResult = Assert.IsType<ProblemHttpResult>(result.Result);

        var (statusCode, problemDetails) = (problemResult.StatusCode, problemResult.ProblemDetails);

        Assert.Equal(StatusCodes.Status403Forbidden, statusCode);

        Assert.Equal(StatusCodes.Status403Forbidden, problemDetails.Status);
        Assert.Equal("Forbidden", problemDetails.Title);
        Assert.Equal("Client not authorized.", problemDetails.Detail);
        Assert.NotNull(problemDetails.Type);
        Assert.Null(problemDetails.Instance);
        Assert.Empty(problemDetails.Extensions);
    }

    [Fact]
    public async Task ToResultOrProblemAsync_should_throw_given_null_responseMapper_arg()
    {
        // Arrange
        ErrorOr<string> sut = ErrorOrFactory.From(Value);

        // Act
        Func<Task<Results<Ok, ProblemHttpResult>>> task = async () =>
            await Task.FromResult(sut).ToResultOrProblemAsync<string, Ok>(null!);

        // Assert
        ArgumentNullException exception = await Assert.ThrowsAsync<ArgumentNullException>(task);

        Assert.Equal("Value cannot be null. (Parameter 'responseMapper')", exception.Message);
    }
}
