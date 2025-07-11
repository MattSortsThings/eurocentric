using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public static class CityNameTests
{
    public sealed class EqualsMethod : UnitTest
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            const string sutValue = "Liverpool";

            CityName sut = CityName.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            const string sharedValue = "Liverpool";

            CityName sut = CityName.FromValue(sharedValue).Value;
            CityName other = CityName.FromValue(sharedValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_values()
        {
            // Arrange
            const string sutValue = "Liverpool";
            const string otherValue = "Turin";

            CityName sut = CityName.FromValue(sutValue).Value;
            CityName other = CityName.FromValue(otherValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            const string sutValue = "Liverpool";

            CityName sut = CityName.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class FromValueStaticMethod : UnitTest
    {
        [Theory]
        [InlineData("Liverpool")]
        [InlineData("Tel Aviv")]
        [InlineData("Malmö")]
        public void Should_return_instance_with_value_given_non_empty_non_whitespace_string_of_200_chars_or_less(string value)
        {
            // Act
            ErrorOr<CityName> errorsOrResult = CityName.FromValue(value);

            (bool isError, CityName result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);
            Assert.Equal(value, result.Value);
        }

        [Fact]
        public void Should_return_errors_first_of_which_is_Failure_given_empty_string()
        {
            // Arrange
            const string value = "";

            // Act
            ErrorOr<CityName> errorsOrResult = CityName.FromValue(value);

            (bool isError, CityName result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: value });
        }

        [Fact]
        public void Should_return_errors_first_of_which_is_Failure_given_all_whitespace_string()
        {
            // Arrange
            const string value = "        ";

            // Act
            ErrorOr<CityName> errorsOrResult = CityName.FromValue(value);

            (bool isError, CityName result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: value });
        }

        [Fact]
        public void Should_return_errors_first_of_which_is_Failure_given_string_of_more_than_200_chars()
        {
            // Arrange
            string value = new('X', 201);

            // Act
            ErrorOr<CityName> errorsOrResult = CityName.FromValue(value);

            (bool isError, CityName result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal city name value", firstError.Code);
            Assert.Equal("City name value must be a non-empty, non-whitespace string of no more than 200 characters.",
                firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "cityName", Value: string v } && v == value);
        }

        [Fact]
        public void Should_throw_given_null_value_arg()
        {
            // Act
            Action act = () => CityName.FromValue(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);

            Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
        }
    }
}
