using Eurocentric.Components.ErrorHandling;
using Eurocentric.Domain.Functional;
using Eurocentric.UnitTests.TestUtils;
using Eurocentric.UnitTests.TestUtils.Assertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eurocentric.UnitTests.Components.ErrorHandling;

public sealed class DomainErrorExtensionsTests : UnitTest
{
    [Test]
    public async Task ToProblemDetails_should_return_mapped_ProblemDetails_when_instance_is_ConflictError()
    {
        // Arrange
        ConflictError sut = new()
        {
            Title = "Conflict",
            Detail = "The request conflicted with the system state.",
            Metadata = null,
        };

        // Act
        ProblemDetails result = sut.ToProblemDetails();

        // Assert
        await Assert
            .That(result)
            .HasTitle(sut.Title)
            .And.HasDetail(sut.Detail)
            .And.HasStatus(StatusCodes.Status409Conflict)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.10")
            .And.HasEmptyExtensions()
            .And.HasNullInstance();
    }

    [Test]
    public async Task ToProblemDetails_should_return_mapped_ProblemDetails_when_instance_is_NotFoundError()
    {
        // Arrange
        Guid resourceId = Guid.Parse("5c1e694b-debd-4e82-a8b5-58a618378d9b");

        NotFoundError sut = new()
        {
            Title = "Not Found",
            Detail = "The requested resource was not found.",
            Metadata = new Dictionary<string, object?> { { nameof(resourceId), resourceId } },
        };

        // Act
        ProblemDetails result = sut.ToProblemDetails();

        // Assert
        await Assert
            .That(result)
            .HasTitle(sut.Title)
            .And.HasDetail(sut.Detail)
            .And.HasStatus(StatusCodes.Status404NotFound)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.5")
            .And.HasExtension("resourceId", resourceId)
            .And.HasNullInstance();
    }

    [Test]
    public async Task ToProblemDetails_should_return_mapped_ProblemDetails_when_instance_is_UnprocessableError()
    {
        // Arrange
        const int valueA = 12345;
        const string valueB = "abc";
        Guid valueC = Guid.Parse("5c1e694b-debd-4e82-a8b5-58a618378d9b");

        UnprocessableError sut = new()
        {
            Title = "Unprocessable",
            Detail = "The request was unprocessable.",
            Metadata = new Dictionary<string, object?>
            {
                { nameof(valueA), valueA },
                { nameof(valueB), valueB },
                { nameof(valueC), valueC },
            },
        };

        // Act
        ProblemDetails result = sut.ToProblemDetails();

        // Assert
        await Assert
            .That(result)
            .HasTitle(sut.Title)
            .And.HasDetail(sut.Detail)
            .And.HasStatus(StatusCodes.Status422UnprocessableEntity)
            .And.HasType("https://tools.ietf.org/html/rfc9110#section-15.5.21")
            .And.HasExtension("valueA", valueA)
            .And.HasExtension("valueB", valueB)
            .And.HasExtension("valueC", valueC)
            .And.HasNullInstance();
    }

    [Test]
    public async Task ToProblemDetails_should_throw_when_instance_is_unsupported_type()
    {
        // Arrange
        UnsupportedError sut = new();

        // Assert
        await Assert
            .That(() => sut.ToProblemDetails())
            .Throws<InvalidOperationException>()
            .WithMessage(
                "Unsupported domain error type: "
                    + "Eurocentric.UnitTests.Components.ErrorHandling.DomainErrorExtensionsTests+UnsupportedError."
            );
    }

    private sealed class UnsupportedError : IDomainError
    {
        public string Title => string.Empty;

        public string Detail => string.Empty;

        public IReadOnlyDictionary<string, object?>? Metadata => null;
    }
}
