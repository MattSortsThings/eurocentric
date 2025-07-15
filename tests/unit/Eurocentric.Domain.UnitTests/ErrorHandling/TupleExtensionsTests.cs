using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utils;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public static class TupleExtensionsTests
{
    public sealed class CombineExtensionMethod2TupleOverload : UnitTest
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
            Error error = Error.Failure();

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
            Error error = Error.Failure();

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
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();

            ErrorOr<int> item1 = errorA;
            ErrorOr<char> item2 = new List<Error> { errorB, errorC };

            Tuple<ErrorOr<int>, ErrorOr<char>> sut = Tuple.Create(item1, item2);

            // Act
            ErrorOr<Tuple<int, char>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { errorA, errorB, errorC }, errors);
        }
    }

    public sealed class CombineExtensionMethod3TupleOverload : UnitTest
    {
        [Fact]
        public void Should_return_tuple_of_values_when_all_items_are_values()
        {
            // Arrange
            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = '2';
            ErrorOr<bool> item3 = true;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, Tuple<int, char, bool> tuple) = (errorsOrTuple.IsError, errorsOrTuple.Value);

            // Assert
            Assert.False(isError);

            Assert.Equal(Tuple.Create(1, '2', true), tuple);
        }

        [Fact]
        public void Should_return_Errors_when_first_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure();

            ErrorOr<int> item1 = error;
            ErrorOr<char> item2 = '2';
            ErrorOr<bool> item3 = true;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equal(new[] { error }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_second_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure();

            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = error;
            ErrorOr<bool> item3 = true;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equal(new[] { error }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_third_item_is_Errors()
        {
            // Arrange
            Error error = Error.Failure();

            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = '2';
            ErrorOr<bool> item3 = error;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equal(new[] { error }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_first_and_second_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            ErrorOr<int> item1 = new List<Error> { errorA, errorD };
            ErrorOr<char> item2 = new List<Error> { errorB, errorC };
            ErrorOr<bool> item3 = true;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { errorA, errorB, errorC, errorD }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_first_and_third_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            ErrorOr<int> item1 = new List<Error> { errorA, errorD };
            ErrorOr<char> item2 = '2';
            ErrorOr<bool> item3 = new List<Error> { errorB, errorC };

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { errorA, errorB, errorC, errorD }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_second_and_third_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            ErrorOr<int> item1 = 1;
            ErrorOr<char> item2 = new List<Error> { errorA, errorD };
            ErrorOr<bool> item3 = new List<Error> { errorB, errorC };

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { errorA, errorB, errorC, errorD }, errors);
        }

        [Fact]
        public void Should_return_Errors_when_all_items_are_Errors()
        {
            // Arrange
            Error errorA = Error.Failure();
            Error errorB = Error.Conflict();
            Error errorC = Error.Validation();
            Error errorD = Error.Unexpected();

            ErrorOr<int> item1 = errorA;
            ErrorOr<char> item2 = new List<Error> { errorB, errorC };
            ErrorOr<bool> item3 = errorD;

            Tuple<ErrorOr<int>, ErrorOr<char>, ErrorOr<bool>> sut = Tuple.Create(item1, item2, item3);

            // Act
            ErrorOr<Tuple<int, char, bool>> errorsOrTuple = sut.Combine();

            (bool isError, List<Error> errors) = (errorsOrTuple.IsError, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);

            Assert.Equivalent(new[] { errorA, errorB, errorC, errorD }, errors);
        }
    }
}
