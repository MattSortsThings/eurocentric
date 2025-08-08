using ErrorOr;
using Eurocentric.Domain.UnitTests.Utils;
using Eurocentric.Domain.UnitTests.Utils.Assertions;
using Eurocentric.Domain.ValueObjects;
using TUnit.Assertions.Enums;

namespace Eurocentric.Domain.UnitTests.ValueObjects;

public sealed class BroadcastDateTests : UnitTest
{
    private const string DateFormat = "yyyy-MM-dd";

    [Test]
    public async Task CompareTo_should_compare_by_Value()
    {
        // Arrange
        BroadcastDate dateA = BroadcastDate.FromValue(DateOnly.ParseExact("2016-01-01", DateFormat)).Value;
        BroadcastDate dateB = BroadcastDate.FromValue(DateOnly.ParseExact("2023-05-01", DateFormat)).Value;
        BroadcastDate dateC = BroadcastDate.FromValue(DateOnly.ParseExact("2023-05-02", DateFormat)).Value;
        BroadcastDate dateD = BroadcastDate.FromValue(DateOnly.ParseExact("2050-12-31", DateFormat)).Value;

        List<BroadcastDate> sut = [dateC, dateB, dateD, dateA];

        // Act
        sut.Sort(Comparer<BroadcastDate>.Default);

        await Assert.That(sut).IsEquivalentTo([dateA, dateB, dateC, dateD], CollectionOrdering.Matching);
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_instance()
    {
        // Arrange
        DateOnly sutValue = DateOnly.ParseExact("2025-01-01", DateFormat);

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_Value_properties_are_equal()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.ParseExact("2025-01-01", DateFormat);

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).Value;
        BroadcastDate other = BroadcastDate.FromValue(sharedValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_Value_properties_are_unequal()
    {
        // Arrange
        DateOnly sutValue = DateOnly.ParseExact("2025-01-01", DateFormat);
        DateOnly otherValue = DateOnly.ParseExact("2050-12-31", DateFormat);

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;
        BroadcastDate other = BroadcastDate.FromValue(otherValue).Value;

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_given_null_other_arg()
    {
        // Arrange
        DateOnly sutValue = DateOnly.ParseExact("2025-01-01", DateFormat);

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).Value;

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("2016-01-01")]
    [Arguments("2025-05-17")]
    [Arguments("2050-12-31")]
    public async Task FromValue_should_return_instance_with_provided_value(string dateValue)
    {
        // Arrange
        DateOnly date = DateOnly.ParseExact(dateValue, DateFormat);

        // Act
        ErrorOr<BroadcastDate> result = BroadcastDate.FromValue(date);

        // Assert
        await Assert.That(result.IsError).IsFalse();

        // Assert
        await Assert.That(result.Value)
            .HasMember(broadcastDate => broadcastDate.Value).EqualTo(date);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_date_with_year_earlier_than_2016()
    {
        // Arrange
        const string dateValue = "2015-12-31";

        DateOnly value = DateOnly.ParseExact(dateValue, DateFormat);

        // Act
        ErrorOr<BroadcastDate> result = BroadcastDate.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal broadcast date value")
            .And.HasDescription("Broadcast date value must have a year between 2016 and 2050.")
            .And.HasMetadataEntry("broadcastDate", dateValue);
    }

    [Test]
    public async Task FromValue_should_return_Errors_when_value_is_date_with_year_later_than_2050()
    {
        // Arrange
        const string dateValue = "2051-01-01";

        DateOnly value = DateOnly.ParseExact(dateValue, DateFormat);

        // Act
        ErrorOr<BroadcastDate> result = BroadcastDate.FromValue(value);

        // Assert
        await Assert.That(result.IsError).IsTrue();

        await Assert.That(result.Value).IsNull();

        await Assert.That(result.FirstError)
            .HasType(ErrorType.Failure)
            .And.HasCode("Illegal broadcast date value")
            .And.HasDescription("Broadcast date value must have a year between 2016 and 2050.")
            .And.HasMetadataEntry("broadcastDate", dateValue);
    }
}
