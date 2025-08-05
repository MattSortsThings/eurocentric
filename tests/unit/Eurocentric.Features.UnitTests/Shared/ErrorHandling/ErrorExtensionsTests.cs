using ErrorOr;
using Eurocentric.Features.Shared.ErrorHandling;
using Eurocentric.Features.UnitTests.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.Features.UnitTests.Shared.ErrorHandling;

public sealed class ErrorExtensionsTests : UnitTest
{
    private const string TestValue = "Cha Cha Cha";

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_422_when_Type_is_Failure()
    {
        // Arrange
        Error sut = Error.Failure("Illegal value",
            "Value is illegal.",
            new Dictionary<string, object> { ["value"] = TestValue });

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        // Assert
        await Assert.That(statusCode).IsEqualTo(422);
        await Assert.That(problemDetails.Status).IsEqualTo(422);
        await Assert.That(problemDetails.Title).IsEqualTo("Illegal value");
        await Assert.That(problemDetails.Detail).IsEqualTo("Value is illegal.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "value", Value: TestValue });
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_500_when_Type_is_Unexpected()
    {
        // Arrange
        Error sut = Error.Unexpected("Unexpected error", "Something went wrong.");

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(500);
        await Assert.That(problemDetails.Status).IsEqualTo(500);
        await Assert.That(problemDetails.Title).IsEqualTo("Unexpected error");
        await Assert.That(problemDetails.Detail).IsEqualTo("Something went wrong.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).IsEmpty();
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_400_when_Type_is_Validation()
    {
        // Arrange
        Error sut = Error.Validation("Invalid value",
            "Value is invalid.",
            new Dictionary<string, object> { ["value"] = TestValue });

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(400);
        await Assert.That(problemDetails.Status).IsEqualTo(400);
        await Assert.That(problemDetails.Title).IsEqualTo("Invalid value");
        await Assert.That(problemDetails.Detail).IsEqualTo("Value is invalid.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "value", Value: TestValue });
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_409_when_Type_is_Conflict()
    {
        // Arrange
        Error sut = Error.Conflict("Value conflict",
            "Value is not unique.",
            new Dictionary<string, object> { ["value"] = TestValue });

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(409);
        await Assert.That(problemDetails.Status).IsEqualTo(409);
        await Assert.That(problemDetails.Title).IsEqualTo("Value conflict");
        await Assert.That(problemDetails.Detail).IsEqualTo("Value is not unique.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "value", Value: TestValue });
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_404_when_Type_is_NotFound()
    {
        // Arrange
        Error sut = Error.NotFound("Value not found",
            "Value does not exist.",
            new Dictionary<string, object> { ["value"] = TestValue });

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(404);
        await Assert.That(problemDetails.Status).IsEqualTo(404);
        await Assert.That(problemDetails.Title).IsEqualTo("Value not found");
        await Assert.That(problemDetails.Detail).IsEqualTo("Value does not exist.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).Contains(kvp => kvp is { Key: "value", Value: TestValue });
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_401_when_Type_is_Unauthorized()
    {
        // Arrange
        Error sut = Error.Unauthorized("Unauthorized", "Client is not authenticated.");

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(401);
        await Assert.That(problemDetails.Status).IsEqualTo(401);
        await Assert.That(problemDetails.Title).IsEqualTo("Unauthorized");
        await Assert.That(problemDetails.Detail).IsEqualTo("Client is not authenticated.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).IsEmpty();
    }

    [Test]
    public async Task ToProblemHttpResult_should_return_ProblemHttpResult_with_status_code_403_when_Type_is_Forbidden()
    {
        // Arrange
        Error sut = Error.Forbidden("Forbidden", "Client is not authorized.");

        // Act
        ProblemHttpResult result = sut.ToProblemHttpResult();

        // Assert
        (int statusCode, ProblemDetails problemDetails) = (result.StatusCode, result.ProblemDetails);

        await Assert.That(statusCode).IsEqualTo(403);
        await Assert.That(problemDetails.Status).IsEqualTo(403);
        await Assert.That(problemDetails.Title).IsEqualTo("Forbidden");
        await Assert.That(problemDetails.Detail).IsEqualTo("Client is not authorized.");
        await Assert.That(problemDetails.Type).StartsWith("https://");
        await Assert.That(problemDetails.Instance).IsNull();
        await Assert.That(problemDetails.Extensions).IsEmpty();
    }
}
