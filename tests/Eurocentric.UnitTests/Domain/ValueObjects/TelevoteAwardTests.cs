using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class TelevoteAwardTests : UnitTest
{
    private const PointsValue ArbitraryPointsValue = PointsValue.Twelve;

    private static readonly CountryId ArbitraryCountryId = CountryId.FromValue(
        Guid.Parse("cc37b546-915d-4795-9095-7471cdaeda28")
    );

    [Test]
    public async Task Constructor_should_throw_given_null_votingCountryId_arg()
    {
        // Assert
        await Assert
            .That(() => _ = new TelevoteAward(null!, ArbitraryPointsValue))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'votingCountryId')");
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        TelevoteAward sut = new(ArbitraryCountryId, ArbitraryPointsValue);

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_TelevoteAward_with_equal_values()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_TelevoteAward_with_unequal_VotingCountryId()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        CountryId otherVotingCountryId = CountryId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sutVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(otherVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_TelevoteAward_with_unequal_PointsValue()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sutPointsValue = PointsValue.Twelve;
        const PointsValue otherPointsValue = PointsValue.Zero;

        TelevoteAward sut = new(sharedVotingCountryId, sutPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, otherPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_TelevoteAward()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        JuryAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        TelevoteAward sut = new(ArbitraryCountryId, ArbitraryPointsValue);

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_TelevoteAward_with_equal_values()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_TelevoteAward_with_unequal_VotingCountryId()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        CountryId otherVotingCountryId = CountryId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sutVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(otherVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_TelevoteAward_with_unequal_PointsValue()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sutPointsValue = PointsValue.Twelve;
        const PointsValue otherPointsValue = PointsValue.Zero;

        TelevoteAward sut = new(sharedVotingCountryId, sutPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, otherPointsValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_TelevoteAward()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        JuryAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_TelevoteAward_with_equal_values()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_TelevoteAward_with_unequal_VotingCountryId()
    {
        // Arrange
        CountryId sutVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        CountryId otherVotingCountryId = CountryId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sutVotingCountryId, sharedPointsValue);
        TelevoteAward other = new(otherVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_TelevoteAward_with_unequal_PointsValue()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sutPointsValue = PointsValue.Twelve;
        const PointsValue otherPointsValue = PointsValue.Zero;

        TelevoteAward sut = new(sharedVotingCountryId, sutPointsValue);
        TelevoteAward other = new(sharedVotingCountryId, otherPointsValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_TelevoteAward()
    {
        // Arrange
        CountryId sharedVotingCountryId = CountryId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const PointsValue sharedPointsValue = PointsValue.Five;

        TelevoteAward sut = new(sharedVotingCountryId, sharedPointsValue);
        JuryAward other = new(sharedVotingCountryId, sharedPointsValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
