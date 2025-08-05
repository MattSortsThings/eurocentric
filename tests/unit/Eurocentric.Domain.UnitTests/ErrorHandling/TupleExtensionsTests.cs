using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utils;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public sealed class TupleExtensionsTests : UnitTest
{
    [Test]
    public async Task Combine_2Tuple_should_return_values_with_both_items_are_values()
    {
        // Arrange
        const string firstValue = "FIRST";
        const int secondValue = 2;

        ErrorOr<string> first = firstValue;
        ErrorOr<int> second = secondValue;

        Tuple<ErrorOr<string>, ErrorOr<int>> sut = Tuple.Create(first, second);

        // Act
        ErrorOr<Tuple<string, int>> result = sut.Combine();

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value).IsTypeOf<Tuple<string, int>>()
            .And.HasMember(tuple => tuple.Item1).EqualTo(firstValue)
            .And.HasMember(tuple => tuple.Item2).EqualTo(secondValue);
    }

    [Test]
    public async Task Combine_2Tuple_should_return_Errors_when_first_item_is_Errors()
    {
        // Arrange
        Error error = Error.Conflict("Conflict");

        ErrorOr<string> first = error;
        ErrorOr<int> second = 2;

        Tuple<ErrorOr<string>, ErrorOr<int>> sut = Tuple.Create(first, second);

        // Act
        ErrorOr<Tuple<string, int>> result = sut.Combine();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors).HasSingleItem()
            .And.Contains(error);
    }

    [Test]
    public async Task Combine_2Tuple_should_return_Errors_when_second_item_is_Errors()
    {
        // Arrange
        Error error = Error.Conflict("Conflict");

        ErrorOr<string> first = "FIRST";
        ErrorOr<int> second = error;

        Tuple<ErrorOr<string>, ErrorOr<int>> sut = Tuple.Create(first, second);

        // Act
        ErrorOr<Tuple<string, int>> result = sut.Combine();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors).HasSingleItem()
            .And.Contains(error);
    }

    [Test]
    public async Task Combine_2Tuple_should_return_Errors_when_both_items_are_Errors()
    {
        // Arrange
        Error errorA = Error.Conflict("Conflict");
        Error errorB = Error.Unexpected("Unexpected");
        Error errorC = Error.Failure("Failure");

        ErrorOr<string> first = new List<Error> { errorA, errorC };
        ErrorOr<int> second = new List<Error> { errorA, errorB };

        Tuple<ErrorOr<string>, ErrorOr<int>> sut = Tuple.Create(first, second);

        // Act
        ErrorOr<Tuple<string, int>> result = sut.Combine();

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.Errors)
            .IsEquivalentTo(new List<Error> { errorA, errorA, errorB, errorC }, CollectionOrdering.Any);
    }
}
