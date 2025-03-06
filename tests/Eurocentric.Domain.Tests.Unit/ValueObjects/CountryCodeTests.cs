using ErrorOr;
using Eurocentric.Domain.Tests.Unit.Utils;
using Eurocentric.Domain.Tests.Unit.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Tests.Unit.ValueObjects;

public static class CountryCodeTests
{
    private static void ShouldHaveValue(this CountryCode subject, string expectedValue) =>
        Assert.Equal(expectedValue, subject.Value);

    public sealed class CompareToMethod : UnitTest
    {
        [Fact]
        public void Should_return_zero_when_instance_given_itself_as_other_arg()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");

            // Act
            int result = sut.CompareTo(sut);

            // Assert
            result.ShouldBeZero();
        }

        [Fact]
        public void Should_return_zero_when_instance_and_other_arg_have_equal_Value()
        {
            // Arrange
            const string sharedValue = "AT";

            CountryCode sut = CountryCode.FromValue(sharedValue);
            CountryCode other = CountryCode.FromValue(sharedValue);

            // Act
            int result = sut.CompareTo(other);

            // Assert
            result.ShouldBeZero();
        }

        [Fact]
        public void Should_return_negative_value_when_instance_Value_precedes_other_arg_Value()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");
            CountryCode other = CountryCode.FromValue("SE");

            // Act
            int result = sut.CompareTo(other);

            // Assert
            result.ShouldBeNegative();
        }

        [Fact]
        public void Should_return_positive_value_when_instance_Value_follows_other_arg_Value()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("SE");
            CountryCode other = CountryCode.FromValue("AT");

            // Act
            int result = sut.CompareTo(other);

            // Assert
            result.ShouldBePositive();
        }

        [Fact]
        public void Should_return_positive_value_when_other_arg_is_null()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");

            // Act
            int result = sut.CompareTo(null);

            // Assert
            result.ShouldBePositive();
        }
    }

    public sealed class EqualsMethod : UnitTest
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other_arg()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");

            // Act
            bool result = sut.Equals(sut);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_arg_have_equal_Value()
        {
            // Arrange
            const string sharedValue = "AT";

            CountryCode sut = CountryCode.FromValue(sharedValue);
            CountryCode other = CountryCode.FromValue(sharedValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_arg_have_unequal_Value()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");
            CountryCode other = CountryCode.FromValue("SE");

            // Act
            bool result = sut.Equals(other);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            CountryCode sut = CountryCode.FromValue("AT");

            // Act
            bool result = sut.Equals(null);

            // Assert
            result.ShouldBeFalse();
        }
    }

    public sealed class CreateStaticMethod : UnitTest
    {
        [Theory]
        [InlineData("AT")]
        [InlineData("BA")]
        [InlineData("CZ")]
        [InlineData("EE")]
        [InlineData("XX")]
        public void Should_return_CountryCode_when_value_arg_is_string_of_2_upper_case_letters(string value)
        {
            // Act
            (bool isError, CountryCode result) = CountryCode.Create(value).ParseAsSuccess();

            // Assert
            isError.ShouldBeFalse();
            result.ShouldHaveValue(value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("Aa")]
        [InlineData("aA")]
        [InlineData("aa")]
        [InlineData("12")]
        [InlineData("AAA")]
        [InlineData("United Kingdom")]
        public void Should_return_errors_when_value_arg_is_not_string_of_2_upper_case_letters(string value)
        {
            // Act
            (bool isError, Error firstError) = CountryCode.Create(value).ParseAsError();

            // Assert
            isError.ShouldBeTrue();
            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country code");
            firstError.ShouldHaveDescription("Country code value must be a string of 2 upper-case letters.");
            firstError.ShouldHaveMetadata("countryCode", value);
        }

        [Fact]
        public void Should_throw_when_value_arg_is_null()
        {
            // Act
            Action action = () => CountryCode.Create(null!);

            // Assert
            ArgumentNullException exception = action.ShouldThrow<ArgumentNullException>();
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'value')");
        }
    }

    public sealed class FromValueStaticMethod : UnitTest
    {
        [Theory]
        [InlineData("AT")]
        [InlineData("")]
        [InlineData("A")]
        [InlineData("aa")]
        [InlineData("12")]
        [InlineData("AAA")]
        [InlineData("United Kingdom")]
        public void Should_return_CountryCode_from_any_value(string value)
        {
            // Act
            CountryCode result = CountryCode.FromValue(value);

            // Assert
            result.ShouldHaveValue(value);
        }

        [Fact]
        public void Should_throw_when_value_arg_is_null()
        {
            // Act
            Action action = () => CountryCode.FromValue(null!);

            // Assert
            ArgumentNullException exception = action.ShouldThrow<ArgumentNullException>();
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'value')");
        }
    }
}
