using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public static class TupleExtensionsTests
{
    public sealed class Combine2TupleExtensionMethod : UnitTest
    {
        [Fact]
        public void Should_return_tuple_of_values_when_both_items_are_values()
        {
            // Arrange
            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = '2';

            Tuple<ErrorOr<int>, ErrorOr<char>> sut = Tuple.Create(item1, item2);

            // Act
            ErrorOr<Tuple<int, char>> errorsOrTuple = sut.Combine();

            (bool isError, Tuple<int, char> tuple) = (errorsOrTuple.IsError, errorsOrTuple.Value);

            // Assert
            Assert.False(isError);

            Assert.Equal(Tuple.Create(1, '2'), tuple);
        }

        [Fact]
        public void Should_return_Errors_when_first_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure("Failure");

            ErrorOr<int> item1 = error;
            ErrorOr<char> item2 = '2';

            Tuple<ErrorOr<int>, ErrorOr<char>> sut = Tuple.Create(item1, item2);

            // Act
            ErrorOr<Tuple<int, char>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equal(new[] { error }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_second_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure("Failure");

            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = error;

            Tuple<ErrorOr<int>, ErrorOr<char>> sut = Tuple.Create(item1, item2);

            // Act
            ErrorOr<Tuple<int, char>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equal(new[] { error }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_both_items_are_Errors()
        {
            // Arrange
            Error error1Of3 = Error.Failure("Failure");
            Error error2Of3 = Error.Conflict("Conflict");
            Error error3Of3 = Error.Validation("Validation");

            ErrorOr<int> item1 = error1Of3;
            ErrorOr<char> item2 = new List<Error> { error2Of3, error3Of3 };

            Tuple<ErrorOr<int>, ErrorOr<char>> sut = Tuple.Create(item1, item2);

            // Act
            ErrorOr<Tuple<int, char>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { error1Of3, error2Of3, error3Of3 }, errors);
        }
    }
}
