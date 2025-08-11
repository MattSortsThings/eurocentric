using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utils;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public sealed class ListExtensionsTests : UnitTest
{
    [Test]
    public async Task Collect_should_return_ordered_list_of_values_when_all_items_are_values()
    {
        // Arrange
        const char firstValue = 'G';
        const char secondValue = 'B';
        const char thirdValue = '?';

        ErrorOr<char> first = firstValue;
        ErrorOr<char> second = secondValue;
        ErrorOr<char> third = thirdValue;

        List<ErrorOr<char>> sut = [first, second, third];

        // Act
        ErrorOr<List<char>> result = sut.Collect();

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value).IsNotNull()
            .And.IsEquivalentTo([firstValue, secondValue, thirdValue], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Collect_should_return_ordered_list_of_Errors_when_single_item_is_Errors()
    {
        // Arrange
        Error error = Error.Conflict("Conflict");

        const char secondValue = 'B';
        const char thirdValue = '?';

        ErrorOr<char> first = error;
        ErrorOr<char> second = secondValue;
        ErrorOr<char> third = thirdValue;

        List<ErrorOr<char>> sut = [first, second, third];

        // Act
        ErrorOr<List<char>> result = sut.Collect();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors).IsEquivalentTo(new List<Error> { error });
    }

    [Test]
    public async Task Collect_should_return_ordered_list_of_Errors_when_multiple_items_are_Errors()
    {
        // Arrange
        Error errorA = Error.Conflict("Conflict");
        Error errorB = Error.Unexpected("Unexpected");

        const char secondValue = 'B';

        ErrorOr<char> first = errorA;
        ErrorOr<char> second = secondValue;
        ErrorOr<char> third = errorB;

        List<ErrorOr<char>> sut = [first, second, third];

        // Act
        ErrorOr<List<char>> result = sut.Collect();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors).IsEquivalentTo((List<Error>) [errorA, errorB], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Collect_should_return_ordered_list_of_Errors_when_all_items_are_Errors()
    {
        // Arrange
        Error errorA = Error.Conflict("Conflict");
        Error errorB = Error.Unexpected("Unexpected");
        Error errorC = Error.Failure("Failure");

        ErrorOr<char> first = new List<Error> { errorA, errorC };
        ErrorOr<char> second = errorA;
        ErrorOr<char> third = new List<Error> { errorA, errorB };

        List<ErrorOr<char>> sut = [first, second, third];

        // Act
        ErrorOr<List<char>> result = sut.Collect();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors)
            .IsEquivalentTo((List<Error>) [errorA, errorC, errorA, errorA, errorB], CollectionOrdering.Matching);
    }
}
