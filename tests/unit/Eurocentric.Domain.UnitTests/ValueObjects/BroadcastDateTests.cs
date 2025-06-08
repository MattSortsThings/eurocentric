using ErrorOr;
using Eurocentric.Domain.UnitTests.Utilities;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class BroadcastDateTests : UnitTestBase
{
    [Theory]
    [InlineData("2016-01-01")]
    [InlineData("2025-05-17")]
    [InlineData("2016-12-31")]
    public void Should_return_instance_with_value_given_date_between_1_Jan_2016_and_31_Dec_2050(string dateString)
    {
        // Arrange
        DateOnly value = DateOnly.ParseExact(dateString, "yyyy-MM-dd");

        // Act
        ErrorOr<BroadcastDate> errorsOrResult = BroadcastDate.FromValue(value);

        (bool isError, BroadcastDate result) = (errorsOrResult.IsError, errorsOrResult.Value);

        // Assert
        Assert.False(isError);

        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Should_return_errors_first_of_which_is_Failure_given_value_earlier_than_1_Jan_2016()
    {
        // Arrange
        DateOnly value = DateOnly.ParseExact("2015-12-31", "yyyy-MM-dd");

        // Act
        ErrorOr<BroadcastDate> errorsOrResult = BroadcastDate.FromValue(value);

        (bool isError, BroadcastDate result, Error firstError) =
            (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

        // Assert
        Assert.True(isError);

        Assert.Null(result);

        Assert.Equal(ErrorType.Failure, firstError.Type);
        Assert.Equal("Illegal broadcast date value", firstError.Code);
        Assert.Equal("Broadcast date value must be between 2016-01-01 and 2050-12-31.", firstError.Description);
        Assert.NotNull(firstError.Metadata);
        Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d } && d == value);
    }

    [Fact]
    public void Should_return_errors_first_of_which_is_Failure_given_value_later_than_31_Dec_2050()
    {
        // Arrange
        DateOnly value = DateOnly.ParseExact("2051-01-01", "yyyy-MM-dd");

        // Act
        ErrorOr<BroadcastDate> errorsOrResult = BroadcastDate.FromValue(value);

        (bool isError, BroadcastDate result, Error firstError) =
            (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

        // Assert
        Assert.True(isError);

        Assert.Null(result);

        Assert.Equal(ErrorType.Failure, firstError.Type);
        Assert.Equal("Illegal broadcast date value", firstError.Code);
        Assert.Equal("Broadcast date value must be between 2016-01-01 and 2050-12-31.", firstError.Description);
        Assert.NotNull(firstError.Metadata);
        Assert.Contains(firstError.Metadata, kvp => kvp is { Key: "broadcastDate", Value: DateOnly d } && d == value);
    }

    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            DateOnly sutValue = DateOnly.Parse("2025-01-01");

            BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            DateOnly sharedValue = DateOnly.Parse("2025-01-01");

            BroadcastDate sut = BroadcastDate.FromValue(sharedValue).Value;
            BroadcastDate other = BroadcastDate.FromValue(sharedValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_values()
        {
            // Arrange
            DateOnly sutValue = DateOnly.Parse("2016-01-01");
            DateOnly otherValue = DateOnly.Parse("2050-01-01");

            BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;
            BroadcastDate other = BroadcastDate.FromValue(otherValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            DateOnly sutValue = DateOnly.Parse("2025-01-01");

            BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }
}
