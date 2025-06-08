using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utilities;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public sealed class ErrorHandlingExtensionsTests : UnitTestBase
{
    public sealed class CollectExtensionMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_list_of_values_when_all_items_are_values()
        {
            // Arrange
            ErrorOr<string> first = "first";
            ErrorOr<string> second = "second";
            ErrorOr<string> third = "third";

            List<ErrorOr<string>> sut = [first, second, third];

            // Act
            ErrorOr<List<string>> result = sut.Collect();

            var (isError, list) = (result.IsError, result.Value);

            Assert.False(isError);
            Assert.Equal(["first", "second", "third"], list);
        }

        [Fact]
        public void Should_return_list_of_errors_when_any_item_is_errors_scenario_1()
        {
            // Arrange
            Error error1Of2 = Error.Failure("Error 1 of 2");
            Error error2Of2 = Error.Failure("Error 2 of 2");

            ErrorOr<string> first = error1Of2;
            ErrorOr<string> second = "second";
            ErrorOr<string> third = error2Of2;

            List<ErrorOr<string>> sut = [first, second, third];

            // Act
            ErrorOr<List<string>> result = sut.Collect();

            var (isError, list, errors) = (result.IsError, result.Value, result.ErrorsOrEmptyList);

            Assert.True(isError);

            Assert.Null(list);

            Assert.Equivalent(new[] { error1Of2, error2Of2 }, errors);
        }

        [Fact]
        public void Should_return_list_of_errors_when_any_item_is_errors_scenario_2()
        {
            // Arrange
            Error error1Of2 = Error.Failure("Error 1 of 2");
            Error error2Of2 = Error.Failure("Error 2 of 2");

            ErrorOr<string> first = "first";
            ErrorOr<string> second = "second";
            ErrorOr<string> third = new List<Error> { error1Of2, error2Of2 };

            List<ErrorOr<string>> sut = [first, second, third];

            // Act
            ErrorOr<List<string>> result = sut.Collect();

            var (isError, list, errors) = (result.IsError, result.Value, result.ErrorsOrEmptyList);

            Assert.True(isError);

            Assert.Null(list);

            Assert.Equivalent(new[] { error1Of2, error2Of2 }, errors);
        }

        [Fact]
        public void Should_return_list_of_errors_when_any_item_is_errors_scenario_3()
        {
            // Arrange
            Error error1Of3 = Error.Failure("Error 1 of 3");
            Error error2Of3 = Error.Failure("Error 2 of 3");
            Error error3Of3 = Error.Failure("Error 3 of 3");

            ErrorOr<string> first = error1Of3;
            ErrorOr<string> second = "second";
            ErrorOr<string> third = new List<Error> { error2Of3, error3Of3 };

            List<ErrorOr<string>> sut = [first, second, third];

            // Act
            ErrorOr<List<string>> result = sut.Collect();

            var (isError, list, errors) = (result.IsError, result.Value, result.ErrorsOrEmptyList);

            Assert.True(isError);

            Assert.Null(list);

            Assert.Equivalent(new[] { error1Of3, error2Of3, error3Of3 }, errors);
        }
    }

    public sealed class CombineExtensionMethod2TupleOverload : UnitTestBase
    {
        [Fact]
        public void Should_return_tuple_of_values_when_both_tuple_members_are_values()
        {
            // Arrange
            ErrorOr<int> first = 1;
            ErrorOr<string> second = "second";

            Tuple<ErrorOr<int>, ErrorOr<string>> sut = Tuple.Create(first, second);

            // Act
            ErrorOr<Tuple<int, string>> result = sut.Combine();

            var (isError, tuple) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(tuple);

            Assert.Equal(1, tuple.Item1);
            Assert.Equal("second", tuple.Item2);
        }

        [Fact]
        public void Should_return_errors_when_first_tuple_member_is_errors()
        {
            // Arrange
            Error intError = Error.Failure("Integer error");

            ErrorOr<int> first = intError;
            ErrorOr<string> second = "second";

            Tuple<ErrorOr<int>, ErrorOr<string>> sut = Tuple.Create(first, second);

            // Act
            ErrorOr<Tuple<int, string>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equal([intError], errors);
        }

        [Fact]
        public void Should_return_errors_when_second_tuple_member_is_errors()
        {
            // Arrange
            Error stringError = Error.Failure("String error");

            ErrorOr<int> first = 1;
            ErrorOr<string> second = stringError;

            Tuple<ErrorOr<int>, ErrorOr<string>> sut = Tuple.Create(first, second);

            // Act
            ErrorOr<Tuple<int, string>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equal([stringError], errors);
        }

        [Fact]
        public void Should_return_errors_when_both_tuple_members_are_errors()
        {
            // Arrange
            Error intError = Error.Failure("Integer error");
            Error stringError = Error.Failure("String error");

            ErrorOr<int> first = intError;
            ErrorOr<string> second = stringError;

            Tuple<ErrorOr<int>, ErrorOr<string>> sut = Tuple.Create(first, second);

            // Act
            ErrorOr<Tuple<int, string>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equivalent(new[] { intError, stringError }, errors);
        }
    }

    public sealed class CombineExtensionMethod3TupleOverload : UnitTestBase
    {
        [Fact]
        public void Should_return_tuple_of_values_when_all_tuple_members_are_values()
        {
            // Arrange
            ErrorOr<int> first = 1;
            ErrorOr<string> second = "second";
            ErrorOr<char> third = '3';

            Tuple<ErrorOr<int>, ErrorOr<string>, ErrorOr<char>> sut = Tuple.Create(first, second, third);

            // Act
            ErrorOr<Tuple<int, string, char>> result = sut.Combine();

            var (isError, tuple) = (result.IsError, result.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(tuple);

            Assert.Equal(1, tuple.Item1);
            Assert.Equal("second", tuple.Item2);
            Assert.Equal('3', tuple.Item3);
        }

        [Fact]
        public void Should_return_errors_when_first_tuple_member_is_errors()
        {
            // Arrange
            Error intError = Error.Failure("Integer error");

            ErrorOr<int> first = intError;
            ErrorOr<string> second = "second";
            ErrorOr<char> third = '3';

            Tuple<ErrorOr<int>, ErrorOr<string>, ErrorOr<char>> sut = Tuple.Create(first, second, third);

            // Act
            ErrorOr<Tuple<int, string, char>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equal([intError], errors);
        }

        [Fact]
        public void Should_return_errors_when_second_tuple_member_is_errors()
        {
            // Arrange
            Error stringError = Error.Failure("String error");

            ErrorOr<int> first = 1;
            ErrorOr<string> second = stringError;
            ErrorOr<char> third = '3';

            Tuple<ErrorOr<int>, ErrorOr<string>, ErrorOr<char>> sut = Tuple.Create(first, second, third);

            // Act
            ErrorOr<Tuple<int, string, char>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equal([stringError], errors);
        }

        [Fact]
        public void Should_return_errors_when_third_tuple_member_is_errors()
        {
            // Arrange
            Error charError = Error.Failure("char error");

            ErrorOr<int> first = 1;
            ErrorOr<string> second = "second";
            ErrorOr<char> third = charError;

            Tuple<ErrorOr<int>, ErrorOr<string>, ErrorOr<char>> sut = Tuple.Create(first, second, third);

            // Act
            ErrorOr<Tuple<int, string, char>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equal([charError], errors);
        }

        [Fact]
        public void Should_return_errors_when_all_tuple_members_are_errors()
        {
            // Arrange
            Error intError = Error.Failure("Integer error");
            Error stringError = Error.Failure("String error");
            Error charError = Error.Failure("Char error");

            ErrorOr<int> first = intError;
            ErrorOr<string> second = stringError;
            ErrorOr<char> third = charError;

            Tuple<ErrorOr<int>, ErrorOr<string>, ErrorOr<char>> sut = Tuple.Create(first, second, third);

            // Act
            ErrorOr<Tuple<int, string, char>> result = sut.Combine();

            var (isError, tuple, errors) = (result.IsError, result.Value, result.Errors);

            // Assert
            Assert.True(isError);

            Assert.Null(tuple);

            Assert.Equivalent(new[] { intError, stringError, charError }, errors);
        }
    }
}
