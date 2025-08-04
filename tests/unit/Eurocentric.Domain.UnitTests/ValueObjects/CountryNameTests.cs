using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.AssertConditions.Throws;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class CountryNameTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        CountryName nameA = CountryName.FromValue("Austria").Value;
        CountryName nameB = CountryName.FromValue("Belgium").Value;
        CountryName nameC = CountryName.FromValue("Czechia").Value;
        CountryName nameD = CountryName.FromValue("Denmark").Value;

        List<CountryName> sut = [nameC, nameB, nameD, nameA];

        // Act
        sut.Sort(Comparer<CountryName>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([nameA, nameB, nameC, nameD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        const string sutValue = "AT";

        CountryName sut = CountryName.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        const string sharedValue = "AT";

        CountryName sut = CountryName.FromValue(sharedValue).Value;
        CountryName other = CountryName.FromValue(sharedValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        const string sutValue = "AT";
        const string otherValue = "XX";

        CountryName sut = CountryName.FromValue(sutValue).Value;
        CountryName other = CountryName.FromValue(otherValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        const string sutValue = "AT";

        CountryName sut = CountryName.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("A")]
    [Arguments("Austria")]
    [Arguments("Bosnia & Herzegovina")]
    [Arguments("Rest of the World")]
    public async Task FromValue_should_return_instance_with_provided_value(string value)
    {
        // Act
        ErrorOr<CountryName> result = CountryName.FromValue(value);

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
        ErrorOr<CountryName> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country name value")
            .And.HasDescription("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("countryName", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_all_whitespace()
    {
        // Arrange
        const string value = "   ";

        // Act
        ErrorOr<CountryName> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country name value")
            .And.HasDescription("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("countryName", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_longer_than_200_characters()
    {
        // Arrange
        string value = new('X', 201);

        // Act
        ErrorOr<CountryName> result = CountryName.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country name value")
            .And.HasDescription("Country name value must be a non-empty, non-whitespace string of no more than 200 characters.")
            .And.HasMetadataEntry("countryName", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Arrange
        Action act = () => CountryName.FromValue(null!);

        // Assert
        await Assert.That(act)
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')")
            .WithParameterName("value");
    }
}
