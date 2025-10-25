using CSharpFunctionalExtensions;
using Eurocentric.Domain.Core;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class SongTitleTests : UnitTest
{
    private const string ArbitraryValue = "SongTitle";

    [Test]
    public async Task CompareTo_should_return_0_when_other_is_same_instance()
    {
        // Arrange
        SongTitle sut = SongTitle.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(sut);

        // Assert
        await Assert.That(result).IsEqualTo(0);
    }

    [Test]
    public async Task CompareTo_should_return_negative_value_when_instance_Value_precedes_other_Value()
    {
        // Arrange
        const string sutValue = "Wasted Love";
        const string otherValue = "What The Hell Just Happened?";

        SongTitle sut = SongTitle.FromValue(sutValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsNegative();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_Value_precedes_instance_Value()
    {
        // Arrange
        const string sutValue = "Wasted Love";
        const string otherValue = "C'est La Vie";

        SongTitle sut = SongTitle.FromValue(sutValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(otherValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(other);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task CompareTo_should_return_positive_value_when_other_is_null()
    {
        // Arrange
        SongTitle sut = SongTitle.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        int result = sut.CompareTo(null);

        // Assert
        await Assert.That(result).IsPositive();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        SongTitle sut = SongTitle.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_SongTitle_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_SongTitle_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Wasted Love";
        const string otherValue = "What The Hell Just Happened?";

        SongTitle sut = SongTitle.FromValue(sutValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_SongTitle()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
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
        SongTitle sut = SongTitle.FromValue(ArbitraryValue).GetValueOrDefault();

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    [Arguments("RÓA")]
    [Arguments("maman")]
    [Arguments("Volevo essere un duro")]
    public async Task FromValue_should_return_SongTitle_with_provided_Value(string value)
    {
        // Act
        Result<SongTitle, IDomainError> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsSuccess).IsTrue();

        await Assert
            .That(result.GetValueOrDefault())
            .IsNotNull()
            .And.Member(actCode => actCode.Value, source => source.IsEqualTo(value));
    }

    [Test]
    public async Task FromValue_should_fail_given_string_value_with_length_greater_than_200()
    {
        // Arrange
        string value = new('A', 201);

        // Act
        Result<SongTitle, IDomainError> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal song title value")
            .And.HasDetail(
                "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("songTitle", value);
    }

    [Test]
    public async Task FromValue_should_fail_given_empty_string_value()
    {
        // Arrange
        string value = string.Empty;

        // Act
        Result<SongTitle, IDomainError> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal song title value")
            .And.HasDetail(
                "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("songTitle", value);
    }

    [Test]
    [Arguments(" ")]
    [Arguments("   ")]
    public async Task FromValue_should_fail_given_all_whitespace_string_value(string value)
    {
        // Act
        Result<SongTitle, IDomainError> result = SongTitle.FromValue(value);

        // Assert
        await Assert.That(result.IsFailure).IsTrue();

        await Assert.That(result.GetValueOrDefault()).IsNull();

        await Assert
            .That(result.Error)
            .IsTypeOf<UnprocessableError>()
            .And.HasTitle("Illegal song title value")
            .And.HasDetail(
                "Song title value must be a non-empty, non-whitespace string of no more than 200 characters."
            )
            .And.HasExtension("songTitle", value);
    }

    [Test]
    public async Task FromValue_should_throw_given_null_value_arg()
    {
        // Assert
        await Assert
            .That(() => SongTitle.FromValue(null!))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'value')");
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_SongTitle_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_SongTitle_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Wasted Love";
        const string otherValue = "What The Hell Just Happened?";

        SongTitle sut = SongTitle.FromValue(sutValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_SongTitle()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_SongTitle_with_equal_Value()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_SongTitle_with_unequal_Value()
    {
        // Arrange
        const string sutValue = "Wasted Love";
        const string otherValue = "What The Hell Just Happened?";

        SongTitle sut = SongTitle.FromValue(sutValue).GetValueOrDefault();
        SongTitle other = SongTitle.FromValue(otherValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_SongTitle()
    {
        // Arrange
        const string sharedValue = "Wasted Love";

        SongTitle sut = SongTitle.FromValue(sharedValue).GetValueOrDefault();
        CountryCode other = CountryCode.FromValue(sharedValue).GetValueOrDefault();

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
