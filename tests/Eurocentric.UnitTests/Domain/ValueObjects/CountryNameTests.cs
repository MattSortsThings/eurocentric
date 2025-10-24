using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class CountryNameTests : UnitTest
{
    private const string ArbitraryValue = "CountryName";

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        CountryName sut = CountryName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const string sutValue = "France";
        const string otherValue = "San Marino";

        CountryName sut = CountryName.FromValue(sutValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const string sutValue = "France";
        const string otherValue = "Austria";

        CountryName sut = CountryName.FromValue(sutValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        CountryName sut = CountryName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        CountryName sut = CountryName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_CountryName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_CountryName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "France";
        const string otherValue = "Czechia";

        CountryName sut = CountryName.FromValue(sutValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_CountryName()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        CountryName sut = CountryName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("Austria")]
    [Arguments("France")]
    [Arguments("Rest of the World")]
    public async Task FromValue_should_succeed_and_return_instance_with_provided_value(string value)
    {
        // Act
        Result<CountryName, IDomainError> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(countryCode => countryCode.Value, source => source.IsEqualTo(value));
    }

    [Test]
    public async Task FromValue_should_fail_given_string_value_with_length_greater_than_200()
    {
        // Arrange
        string value = new('A', 201);

        // Act
        Result<CountryName, IDomainError> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country name value")
            .And.HasDetail(
                "Country name value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("countryName", value);
    }

    [Test]
    public async Task FromValue_should_fail_given_empty_string_value()
    {
        // Arrange
        string value = string.Empty;

        // Act
        Result<CountryName, IDomainError> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country name value")
            .And.HasDetail(
                "Country name value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("countryName", value);
    }

    [Test]
    [Arguments(" ")]
    [Arguments("   ")]
    public async Task FromValue_should_fail_given_all_whitespace_string_value(string value)
    {
        // Act
        Result<CountryName, IDomainError> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal country name value")
            .And.HasDetail(
                "Country name value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("countryName", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Assert
        await Assert
            .That(() => CountryName.FromValue(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')");
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_CountryName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_CountryName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "France";
        const string otherValue = "Czechia";

        CountryName sut = CountryName.FromValue(sutValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_CountryName()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_CountryName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_CountryName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "France";
        const string otherValue = "Czechia";

        CountryName sut = CountryName.FromValue(sutValue).GetValueOrDefault();
        CountryName other = CountryName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_not_CountryName()
    {
        // Arrange
        const string sharedValue = "France";

        CountryName sut = CountryName.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
