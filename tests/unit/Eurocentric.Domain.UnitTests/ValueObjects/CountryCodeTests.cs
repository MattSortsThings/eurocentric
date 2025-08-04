using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.AssertConditions.Throws;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class CountryCodeTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        CountryCode codeA = CountryCode.FromValue("AT").Value;
        CountryCode codeB = CountryCode.FromValue("BE").Value;
        CountryCode codeC = CountryCode.FromValue("CZ").Value;
        CountryCode codeD = CountryCode.FromValue("DK").Value;

        List<CountryCode> sut = [codeC, codeB, codeD, codeA];

        // Act
        sut.Sort(Comparer<CountryCode>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([codeA, codeB, codeC, codeD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        const string sutValue = "AT";

        CountryCode sut = CountryCode.FromValue(sutValue).Value;

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

        CountryCode sut = CountryCode.FromValue(sharedValue).Value;
        CountryCode other = CountryCode.FromValue(sharedValue).Value;

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

        CountryCode sut = CountryCode.FromValue(sutValue).Value;
        CountryCode other = CountryCode.FromValue(otherValue).Value;

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

        CountryCode sut = CountryCode.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("AT")]
    [Arguments("XX")]
    public async Task FromValue_should_return_instance_with_provided_value(string value)
    {
        // Act
        ErrorOr<CountryCode> result = CountryCode.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value)
            .HasMember(countryCode => countryCode.Value).EqualTo(value);
    }

    [Test]
    [Arguments("")]
    [Arguments("A")]
    [Arguments("AAA")]
    [Arguments("Aa")]
    [Arguments("aA")]
    [Arguments("  ")]
    [Arguments("00")]
    public async Task FromValue_should_return_Errors_when_value_is_not_string_of_2_upper_case_letters(string value)
    {
        // Act
        ErrorOr<CountryCode> result = CountryCode.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal country code value")
            .And.HasDescription("Country code value must be a string of 2 upper-case letters.")
            .And.HasMetadataEntry("countryCode", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Arrange
        Action act = () => CountryCode.FromValue(null!);

        // Assert
        await Assert.That(act)
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')")
            .WithParameterName("value");
    }
}
