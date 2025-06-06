using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.Utilities;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public sealed class ErrorHandlingExtensionsTests : UnitTestBase
{
    public sealed class CombineExtensionMethod : UnitTestBase
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
}
