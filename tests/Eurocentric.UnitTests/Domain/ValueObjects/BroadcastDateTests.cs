using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class BroadcastDateTests : UnitTest
{
    private static readonly DateOnly ArbitraryValue = DateOnly.Parse("2025-05-17");

    [Test]
    [Arguments("2025-01-01", 2025)]
    [Arguments("2025-12-31", 2025)]
    [Arguments("2016-05-10", 2016)]
    [Arguments("2023-05-17", 2023)]
    public async Task IsIn_should_return_true_when_contestYear_Value_is_equal_to_year_of_instance_Value(
        string dateValue,
        int yearValue
    )
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(DateOnly.Parse(dateValue)).GetValueOrDefault();
        ContestYear contestYear = ContestYear.FromValue(yearValue).GetValueOrDefault();

        // Act
        bool result = sut.IsIn(contestYear);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("2025-01-01", 2026)]
    [Arguments("2025-12-31", 2024)]
    [Arguments("2016-05-10", 2019)]
    [Arguments("2023-05-17", 2016)]
    public async Task IsIn_should_return_false_when_contestYear_Value_is_not_equal_to_year_of_instance_Value_year(
        string dateValue,
        int yearValue
    )
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(DateOnly.Parse(dateValue)).GetValueOrDefault();
        ContestYear contestYear = ContestYear.FromValue(yearValue).GetValueOrDefault();

        // Act
        bool result = sut.IsIn(contestYear);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsIn_should_throw_given_null_contestYear_arg()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).Value;

        // Assert
        await Assert
            .That(() => sut.IsIn(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestYear')");
    }

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        DateOnly sutValue = DateOnly.Parse("2025-05-17");
        DateOnly otherValue = DateOnly.Parse("2025-05-18");

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        DateOnly sutValue = DateOnly.Parse("2025-05-17");
        DateOnly otherValue = DateOnly.Parse("2025-05-16");

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_BroadcastDate_with_equal_Value()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_BroadcastDate_with_unequal_Value()
    {
        // Arrange
        DateOnly sutValue = DateOnly.Parse("2016-01-01");
        DateOnly otherValue = DateOnly.Parse("2050-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_BroadcastDate()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        TestDateValue other = TestDateValue.FromValue(sharedValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("2016-01-01")]
    [Arguments("2025-01-01")]
    [Arguments("2026-05-12")]
    [Arguments("2050-01-01")]
    [Arguments("2050-12-31")]
    public async Task FromValue_should_return_BroadcastDate_with_provided_Value(string valueString)
    {
        // Arrange
        DateOnly value = DateOnly.Parse(valueString);

        // Act
        Result<BroadcastDate, IDomainError> result = BroadcastDate.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNotNull().And.HasValue(value);
    }

    [Test]
    [Arguments("2015-12-31")]
    [Arguments("1999-01-01")]
    public async Task FromValue_should_fail_given_date_with_year_before_2016(string valueString)
    {
        // Arrange
        DateOnly value = DateOnly.Parse(valueString);

        // Act
        Result<BroadcastDate, IDomainError> result = BroadcastDate.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal broadcast date value")
            .And.HasDetail("Broadcast date value must be a date with a year between 2016 and 2050.")
            .And.HasExtension("broadcastDate", value);
    }

    [Test]
    [Arguments("2051-01-01")]
    [Arguments("3000-01-01")]
    public async Task FromValue_should_fail_given_date_with_year_after_2050(string valueString)
    {
        // Arrange
        DateOnly value = DateOnly.Parse(valueString);

        // Act
        Result<BroadcastDate, IDomainError> result = BroadcastDate.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal broadcast date value")
            .And.HasDetail("Broadcast date value must be a date with a year between 2016 and 2050.")
            .And.HasExtension("broadcastDate", value);
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_BroadcastDate_with_equal_Value()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_BroadcastDate_with_unequal_Value()
    {
        // Arrange
        DateOnly sutValue = DateOnly.Parse("2016-01-01");
        DateOnly otherValue = DateOnly.Parse("2050-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_BroadcastDate()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        TestDateValue other = TestDateValue.FromValue(sharedValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_null()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut == null;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_BroadcastDate_with_equal_Value()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_BroadcastDate_with_unequal_Value()
    {
        // Arrange
        DateOnly sutValue = DateOnly.Parse("2016-01-01");
        DateOnly otherValue = DateOnly.Parse("2050-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sutValue).GetValueOrDefault();
        BroadcastDate other = BroadcastDate.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_BroadcastDate()
    {
        // Arrange
        DateOnly sharedValue = DateOnly.Parse("2025-01-01");

        BroadcastDate sut = BroadcastDate.FromValue(sharedValue).GetValueOrDefault();
        TestDateValue other = TestDateValue.FromValue(sharedValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_null()
    {
        // Arrange
        BroadcastDate sut = BroadcastDate.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut != null;

        // Assert
        await Assert.That(result).IsTrue();
    }

    private class TestDateValue : DateOnlyAtomicValueObject
    {
        private TestDateValue(DateOnly value)
            : base(value) { }

        public static TestDateValue FromValue(DateOnly value) => new(value);
    }
}
