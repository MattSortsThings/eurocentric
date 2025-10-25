using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class CountryCodeTests : UnitTest
{
    private const string ArbitraryValue = "AA";

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        CountryCode sut = CountryCode.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const string sutValue = "GB";
        const string otherValue = "XX";

        CountryCode sut = CountryCode.FromValue(sutValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const string sutValue = "GB";
        const string otherValue = "AA";

        CountryCode sut = CountryCode.FromValue(sutValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        CountryCode sut = CountryCode.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        CountryCode sut = CountryCode.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_CountryCode_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_CountryCode_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "GB";
        const string otherValue = "CZ";

        CountryCode sut = CountryCode.FromValue(sutValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_CountryCode()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        CountryCode sut = CountryCode.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("AA")]
    [Arguments("GB")]
    [Arguments("XX")]
    public async Task FromValue_should_return_CountryCode_with_provided_Value(string value)
    {
        // Act
        Result<CountryCode, IDomainError> result = CountryCode.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(countryCode => countryCode.Value, source => source.IsEqualTo(value));
    }

    [Test]
    [Arguments("")]
    [Arguments("A")]
    [Arguments("AAA")]
    [Arguments("AAAA")]
    public async Task FromValue_should_fail_given_string_value_with_length_not_equal_to_2(string value)
    {
        // Act
        Result<CountryCode, IDomainError> result = CountryCode.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country code value")
            .And.HasDetail("Country code value must be a string of 2 upper-case letters.")
            .And.HasExtension("countryCode", value);
    }

    [Test]
    [Arguments("  ")]
    [Arguments("Aa")]
    [Arguments("aA")]
    [Arguments("11")]
    public async Task FromValue_should_fail_given_string_value_not_all_upper_case_letters(string value)
    {
        // Act
        Result<CountryCode, IDomainError> result = CountryCode.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country code value")
            .And.HasDetail("Country code value must be a string of 2 upper-case letters.")
            .And.HasExtension("countryCode", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Assert
        await Assert
            .That(() => CountryCode.FromValue(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')");
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_CountryCode_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_CountryCode_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "GB";
        const string otherValue = "CZ";

        CountryCode sut = CountryCode.FromValue(sutValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_CountryCode()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_CountryCode_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_CountryCode_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "GB";
        const string otherValue = "CZ";

        CountryCode sut = CountryCode.FromValue(sutValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_CountryCode()
    {
        // Arrange
        const string sharedValue = "GB";

        CountryCode sut = CountryCode.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
