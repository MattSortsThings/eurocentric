using ErrorOr;
using Eurocentric.Domain.UnitTests.TestUtils;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class CountryCodeTests : UnitTestBase
{
    public sealed class EqualsMethod : UnitTestBase
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other()
        {
            // Arrange
            const string sutValue = "AT";

            CountryCode sut = CountryCode.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(sut);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_have_equal_values()
        {
            // Arrange
            const string sharedValue = "AT";

            CountryCode sut = CountryCode.FromValue(sharedValue).Value;
            CountryCode other = CountryCode.FromValue(sharedValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_have_unequal_values()
        {
            // Arrange
            const string sutValue = "AT";
            const string otherValue = "XX";

            CountryCode sut = CountryCode.FromValue(sutValue).Value;
            CountryCode other = CountryCode.FromValue(otherValue).Value;

            // Act
            bool result = sut.Equals(other);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            const string sutValue = "AT";

            CountryCode sut = CountryCode.FromValue(sutValue).Value;

            // Act
            bool result = sut.Equals(null);

            // Assert
            Assert.False(result);
        }
    }

    public sealed class FromValueStaticMethod : UnitTestBase
    {
        [Theory]
        [InlineData("AA")]
        [InlineData("GB")]
        [InlineData("XX")]
        public void Should_return_instance_with_value_given_string_of_2_upper_case_letters(string value)
        {
            // Act
            ErrorOr<CountryCode> errorsOrResult = CountryCode.FromValue(value);

            (bool isError, CountryCode result) = (errorsOrResult.IsError, errorsOrResult.Value);

            // Assert
            Assert.False(isError);

            Assert.NotNull(result);
            Assert.Equal(value, result.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("AAA")]
        public void Should_return_errors_first_of_which_is_Failure_given_string_of_length_not_equal_to_2(string value)
        {
            // Act
            ErrorOr<CountryCode> errorsOrResult = CountryCode.FromValue(value);

            (bool isError, CountryCode result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal country code value", firstError.Code);
            Assert.Equal("Country code value must be a string of 2 upper-case letters.", firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: string v } && v == value);
        }

        [Theory]
        [InlineData("Aa")]
        [InlineData("aA")]
        [InlineData("11")]
        [InlineData(" A")]
        public void Should_return_errors_first_of_which_is_Failure_given_string_with_non_upper_case_letter_char(string value)
        {
            // Act
            ErrorOr<CountryCode> errorsOrResult = CountryCode.FromValue(value);

            (bool isError, CountryCode result, Error firstError) =
                (errorsOrResult.IsError, errorsOrResult.Value, errorsOrResult.FirstError);

            // Assert
            Assert.True(isError);

            Assert.Null(result);

            Assert.Equal("Illegal country code value", firstError.Code);
            Assert.Equal("Country code value must be a string of 2 upper-case letters.", firstError.Description);
            Assert.Equal(ErrorType.Failure, firstError.Type);
            Assert.NotNull(firstError.Metadata);
            Assert.Single(firstError.Metadata, kvp => kvp is { Key: "countryCode", Value: string v } && v == value);
        }

        [Fact]
        public void Should_throw_given_null_value_arg()
        {
            // Act
            Action act = () => CountryCode.FromValue(null!);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(act);

            Assert.Equal("Value cannot be null. (Parameter 'value')", exception.Message);
        }
    }
}
