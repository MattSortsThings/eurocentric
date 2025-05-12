using ErrorOr;
using Eurocentric.Domain.ErrorHandling;
using Eurocentric.Domain.UnitTests.TestUtils;

namespace Eurocentric.Domain.UnitTests.ErrorHandling;

public sealed class ErrorOrExtensionsTests : UnitTestBase
{
    public sealed class CombineTwoTupleExtensionMethod : UnitTestBase
    {
        private const string FixedStringValue = "A";
        private const int FixedIntValue = 1;
        private static readonly Error FixedFailureError = Error.Failure("Failure", "Failure occurred.");
        private static readonly Error FixedConflictError = Error.Conflict("Conflict", "Conflict occurred.");

        [Fact]
        public void Should_return_value_tuple_when_both_instance_elements_are_not_errors()
        {
            // Arrange
            ErrorOr<string> first = FixedStringValue;
            ErrorOr<int?> second = FixedIntValue;

            (ErrorOr<string> first, ErrorOr<int?> second) sut = (first, second);

            // Act
            ErrorOr<(string First, int? Second)> errorsOrTuple = sut.Combine();

            (bool isError, (string firstValue, int? secondValue)) = (errorsOrTuple.IsError, errorsOrTuple.Value);

            // Assert
            Assert.False(isError);
            Assert.Equal(FixedStringValue, firstValue);
            Assert.Equal(FixedIntValue, secondValue);
        }

        [Fact]
        public void Should_return_errors_when_instance_first_element_is_errors()
        {
            // Arrange
            ErrorOr<string> first = FixedFailureError;
            ErrorOr<int?> second = FixedIntValue;

            (ErrorOr<string> first, ErrorOr<int?> second) sut = (first, second);

            // Act
            ErrorOr<(string First, int? Second)> errorsOrTuple = sut.Combine();

            (bool isError, (string? firstValue, int? secondValue), List<Error> errors) =
                (errorsOrTuple.IsError, errorsOrTuple.Value, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);
            Assert.Null(firstValue);
            Assert.Null(secondValue);
            Assert.Equal([FixedFailureError], errors);
        }

        [Fact]
        public void Should_return_errors_when_instance_second_element_is_errors()
        {
            // Arrange
            ErrorOr<string> first = FixedStringValue;
            ErrorOr<int?> second = FixedConflictError;

            (ErrorOr<string> first, ErrorOr<int?> second) sut = (first, second);

            // Act
            ErrorOr<(string First, int? Second)> errorsOrTuple = sut.Combine();

            (bool isError, (string? firstValue, int? secondValue), List<Error> errors) =
                (errorsOrTuple.IsError, errorsOrTuple.Value, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);
            Assert.Null(firstValue);
            Assert.Null(secondValue);
            Assert.Equal([FixedConflictError], errors);
        }

        [Fact]
        public void Should_return_errors_when_instance_elements_are_both_errors()
        {
            // Arrange
            ErrorOr<string> first = FixedFailureError;
            ErrorOr<int?> second = FixedConflictError;

            (ErrorOr<string> first, ErrorOr<int?> second) sut = (first, second);

            // Act
            ErrorOr<(string First, int? Second)> errorsOrTuple = sut.Combine();

            (bool isError, (string? firstValue, int? secondValue), List<Error> errors) =
                (errorsOrTuple.IsError, errorsOrTuple.Value, errorsOrTuple.Errors);

            // Assert
            Assert.True(isError);
            Assert.Null(firstValue);
            Assert.Null(secondValue);
            Assert.Equal([FixedFailureError, FixedConflictError], errors);
        }
    }
}
