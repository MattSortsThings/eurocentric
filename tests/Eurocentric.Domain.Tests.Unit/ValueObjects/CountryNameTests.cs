using ErrorOr;
using Eurocentric.Domain.Tests.Unit.Utils;
using Eurocentric.Domain.Tests.Unit.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;

namespace Eurocentric.Domain.Tests.Unit.ValueObjects;

public static class CountryNameTests
{
    private static void ShouldHaveValue(this CountryName subject, string expectedValue) =>
        Assert.Equal(expectedValue, subject.Value);

    public sealed class EqualsMethod : UnitTest
    {
        [Fact]
        public void Should_return_true_when_instance_given_itself_as_other_arg()
        {
            // Arrange
            CountryName sut = CountryName.FromValue("Austria");

            // Act
            bool result = sut.Equals(sut);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_true_when_instance_and_other_arg_have_equal_Value()
        {
            // Arrange
            const string sharedValue = "Austria";

            CountryName sut = CountryName.FromValue(sharedValue);
            CountryName other = CountryName.FromValue(sharedValue);

            // Act
            bool result = sut.Equals(other);

            // Assert
            result.ShouldBeTrue();
        }

        [Fact]
        public void Should_return_false_when_instance_and_other_arg_have_unequal_Value()
        {
            // Arrange
            CountryName sut = CountryName.FromValue("Austria");
            CountryName other = CountryName.FromValue("Sweden");

            // Act
            bool result = sut.Equals(other);

            // Assert
            result.ShouldBeFalse();
        }

        [Fact]
        public void Should_return_false_when_other_arg_is_null()
        {
            // Arrange
            CountryName sut = CountryName.FromValue("Austria");

            // Act
            bool result = sut.Equals(null);

            // Assert
            result.ShouldBeFalse();
        }
    }

    public sealed class CreateStaticMethod : UnitTest
    {
        [Theory]
        [InlineData("Austria")]
        [InlineData("Bosnia & Herzegovina")]
        [InlineData("Czechia")]
        [InlineData("Estonia")]
        [InlineData("Rest of the World")]
        public void Should_return_CountryName_when_value_arg_is_non_empty_non_white_space_string_of_no_more_than_200_chars(
            string value)
        {
            // Act
            (bool isError, CountryName result) = CountryName.Create(value).ParseAsSuccess();

            // Assert
            isError.ShouldBeFalse();
            result.ShouldHaveValue(value);
        }

        [Theory]
        [ClassData(typeof(SadPathTestCases))]
        public void Should_return_errors_when_value_arg_is_not_non_empty_non_white_space_string_of_no_more_than_200_chars(
            string value)
        {
            // Act
            (bool isError, Error firstError) = CountryName.Create(value).ParseAsError();

            // Assert
            isError.ShouldBeTrue();
            firstError.ShouldHaveFailureErrorType();
            firstError.ShouldHaveCode("Invalid country name");
            firstError.ShouldHaveDescription("Country name value must be a non-empty, non-white-space string " +
                                             "of no more than 200 characters.");
            firstError.ShouldHaveMetadata("countryName", value);
        }

        [Fact]
        public void Should_throw_when_value_arg_is_null()
        {
            // Act
            Action action = () => CountryName.Create(null!);

            // Assert
            ArgumentNullException exception = action.ShouldThrow<ArgumentNullException>();
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'value')");
        }

        private sealed class SadPathTestCases : TheoryData<string>
        {
            public SadPathTestCases()
            {
                Add(string.Empty);
                Add(" ");
                Add("     ");
                Add(new string('a', 201));
            }
        }
    }

    public sealed class FromValueStaticMethod : UnitTest
    {
        [Theory]
        [ClassData(typeof(TestCases))]
        public void Should_return_CountryName_from_any_value(string value)
        {
            // Act
            CountryName result = CountryName.FromValue(value);

            // Assert
            result.ShouldHaveValue(value);
        }

        [Fact]
        public void Should_throw_when_value_arg_is_null()
        {
            // Act
            Action action = () => CountryName.FromValue(null!);

            // Assert
            ArgumentNullException exception = action.ShouldThrow<ArgumentNullException>();
            exception.ShouldHaveMessage("Value cannot be null. (Parameter 'value')");
        }

        private sealed class TestCases : TheoryData<string>
        {
            public TestCases()
            {
                Add("Czechia");
                Add("Bosnia & Herzegovina");
                Add("Estonia");
                Add("Rest of the World");
                Add(string.Empty);
                Add(" ");
                Add("     ");
                Add(new string('a', 201));
            }
        }
    }
}
