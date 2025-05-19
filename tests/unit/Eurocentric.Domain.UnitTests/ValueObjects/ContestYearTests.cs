using ErrorOr;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class ContestYearTests : UnitTestBase
{
    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            const int sutValue = 2025;

            ContestYear sut = ContestYear.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            const int sharedValue = 2025;

            ContestYear sut = ContestYear.FromValue(sharedValue).Value;
            ContestYear other = ContestYear.FromValue(sharedValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_values()
        {
            // Arrange
            const int sutValue = 2025;
            const int otherValue = 2050;

            ContestYear sut = ContestYear.FromValue(sutValue).Value;
            ContestYear other = ContestYear.FromValue(otherValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            const int sutValue = 2025;

            ContestYear sut = ContestYear.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class FromValueStaticMethod : UnitTestBase
    {
        [Theory]
        [InlineData(2016)]
        [InlineData(2025)]
        [InlineData(2050)]
        public void Should_return_instance_with_value_given_integer_between_2016_and_2050(int value)
        {
            // Act
            ErrorOr<ContestYear> errorsOrResult = ContestYear.FromValue(value);

            (bool isError, ContestYear result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Should_return_errors_first_of_which_is_Failure_given_integer_less_than_2016()
        {
            // Arrange
            const int value = 2015;

            // Act
            ErrorOr<ContestYear> errorsOrResult = ContestYear.FromValue(value);

            (bool isError, ContestYear result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: int and value });
        }

        [Fact]
        public void Should_return_errors_first_of_which_is_Failure_given_integer_greater_than_2050()
        {
            // Arrange
            const int value = 2051;

            // Act
            ErrorOr<ContestYear> errorsOrResult = ContestYear.FromValue(value);

            (bool isError, ContestYear result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal contest year value", firstError.Code);
            Assert.Equal("Contest year value must be an integer between 2016 and 2050.", firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "contestYear", Value: int and value });
        }
    }
}
