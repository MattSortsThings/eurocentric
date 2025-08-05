using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.AssertConditions.Throws;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class CityNameTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        CityName nameA = CityName.FromValue("Athens").Value;
        CityName nameB = CityName.FromValue("Berlin").Value;
        CityName nameC = CityName.FromValue("Canberra").Value;
        CityName nameD = CityName.FromValue("Dublin").Value;

        List<CityName> sut = [nameC, nameB, nameD, nameA];

        // Act
        sut.Sort(Comparer<CityName>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([nameA, nameB, nameC, nameD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        const string sutValue = "Athens";

        CityName sut = CityName.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        const string sharedValue = "Athens";

        CityName sut = CityName.FromValue(sharedValue).Value;
        CityName other = CityName.FromValue(sharedValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        const string sutValue = "Athens";
        const string otherValue = "Zagreb";

        CityName sut = CityName.FromValue(sutValue).Value;
        CityName other = CityName.FromValue(otherValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        const string sutValue = "Athens";

        CityName sut = CityName.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("A")]
    [Arguments("Athens")]
    [Arguments("San Marino")]
    public async Task FromValue_should_return_instance_with_provided_value(string value)
    {
        // Act
        ErrorOr<CityName> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value)
            .HasMember(countryCode => countryCode.Value).EqualTo(value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_empty_string()
    {
        // Arrange
        const string value = "";

        // Act
        ErrorOr<CityName> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal city name value")
            .And.HasDescription("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("cityName", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_all_whitespace()
    {
        // Arrange
        const string value = "   ";

        // Act
        ErrorOr<CityName> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal city name value")
            .And.HasDescription("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("cityName", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_longer_than_200_characters()
    {
        // Arrange
        string value = new('X', 201);

        // Act
        ErrorOr<CityName> result = CityName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal city name value")
            .And.HasDescription("City name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("cityName", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Arrange
        Action act = () => CityName.FromValue(null!);

        // Assert
        await Assert.That(act)
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')")
            .WithParameterName("value");
    }
}
