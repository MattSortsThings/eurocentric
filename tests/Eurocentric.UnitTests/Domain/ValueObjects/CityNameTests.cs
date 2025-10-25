using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class CityNameTests : UnitTest
{
    private const string ArbitraryValue = "CityName";

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        CityName sut = CityName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const string sutValue = "Liverpool";
        const string otherValue = "Stockholm";

        CityName sut = CityName.FromValue(sutValue).GetValueOrDefault();
        CityName other = CityName.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const string sutValue = "Liverpool";
        const string otherValue = "Basel";

        CityName sut = CityName.FromValue(sutValue).GetValueOrDefault();
        CityName other = CityName.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        CityName sut = CityName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        CityName sut = CityName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_CityName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
        CityName other = CityName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_CityName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Liverpool";
        const string otherValue = "Stockholm";

        CityName sut = CityName.FromValue(sutValue).GetValueOrDefault();
        CityName other = CityName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_CityName()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
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
        CityName sut = CityName.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("Kyiv")]
    [Arguments("Liverpool")]
    [Arguments("Tel Aviv")]
    public async Task FromValue_should_return_CityName_with_provided_Value(string value)
    {
        // Act
        Result<CityName, IDomainError> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(cityCode => cityCode.Value, source => source.IsEqualTo(value));
    }

    [Test]
    public async Task FromValue_should_fail_given_string_value_with_length_greater_than_200()
    {
        // Arrange
        string value = new('A', 201);

        // Act
        Result<CityName, IDomainError> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal city name value")
            .And.HasDetail("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("cityName", value);
    }

    [Test]
    public async Task FromValue_should_fail_given_empty_string_value()
    {
        // Arrange
        string value = string.Empty;

        // Act
        Result<CityName, IDomainError> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal city name value")
            .And.HasDetail("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("cityName", value);
    }

    [Test]
    [Arguments(" ")]
    [Arguments("   ")]
    public async Task FromValue_should_fail_given_all_whitespace_string_value(string value)
    {
        // Act
        Result<CityName, IDomainError> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal city name value")
            .And.HasDetail("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasExtension("cityName", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Assert
        await Assert
            .That(() => CityName.FromValue(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')");
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_CityName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
        CityName other = CityName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_CityName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Liverpool";
        const string otherValue = "Stockholm";

        CityName sut = CityName.FromValue(sutValue).GetValueOrDefault();
        CityName other = CityName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_CityName()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_CityName_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
        CityName other = CityName.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_CityName_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Liverpool";
        const string otherValue = "Stockholm";

        CityName sut = CityName.FromValue(sutValue).GetValueOrDefault();
        CityName other = CityName.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_CityName()
    {
        // Arrange
        const string sharedValue = "Liverpool";

        CityName sut = CityName.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
