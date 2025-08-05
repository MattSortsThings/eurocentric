using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class ContestYearTests : UnitTest
{
    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        ContestYear yearA = ContestYear.FromValue(2016).Value;
        ContestYear yearB = ContestYear.FromValue(2017).Value;
        ContestYear yearC = ContestYear.FromValue(2018).Value;
        ContestYear yearD = ContestYear.FromValue(2019).Value;

        List<ContestYear> sut = [yearC, yearB, yearD, yearA];

        // Act
        sut.Sort(Comparer<ContestYear>.Default);

        // Assert
        await Assert.That(sut).IsEquivalentTo([yearA, yearB, yearC, yearD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        const int sutValue = 2016;

        ContestYear sut = ContestYear.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        const int sharedValue = 2016;

        ContestYear sut = ContestYear.FromValue(sharedValue).Value;
        ContestYear other = ContestYear.FromValue(sharedValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        const int sutValue = 2016;
        const int otherValue = 2050;

        ContestYear sut = ContestYear.FromValue(sutValue).Value;
        ContestYear other = ContestYear.FromValue(otherValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        const int sutValue = 2016;

        ContestYear sut = ContestYear.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments(2016)]
    [Arguments(2025)]
    [Arguments(2050)]
    public async Task FromValue_should_return_instance_with_provided_value(int value)
    {
        // Act
        ErrorOr<ContestYear> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        await Assert.That(result.Value)
            .HasMember(countryCode => countryCode.Value).EqualTo(value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_less_than_2016()
    {
        // Arrange
        const int value = 2015;

        // Act
        ErrorOr<ContestYear> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal contest year value")
            .And.HasDescription("Contest year value must be an integer between 2016 and 2050.")
            .And.HasMetadataEntry("contestYear", value);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_greater_than_2050()
    {
        // Arrange
        const int value = 2051;

        // Act
        ErrorOr<ContestYear> result = ContestYear.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal contest year value")
            .And.HasDescription("Contest year value must be an integer between 2016 and 2050.")
            .And.HasMetadataEntry("contestYear", value);
    }
}
