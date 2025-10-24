using Eurocentric.Domain.Enums;
using Eurocentric.Domain.ValueObjects;
using Eurocentric.UnitTests.TestUtils;

namespace Eurocentric.UnitTests.Domain.ValueObjects;

public sealed class ContestRoleTests : UnitTest
{
    private const ContestRoleType ArbitraryContestRoleType = ContestRoleType.GlobalTelevote;
    private const PointsValue ArbitraryPointsValue = PointsValue.Twelve;

    private static readonly ContestId ArbitraryContestId = ContestId.FromValue(
        Guid.Parse("e2c66954-6c82-4cf9-87ec-0fced2e078d5")
    );

    private static readonly CountryId ArbitraryCountryId = CountryId.FromValue(
        Guid.Parse("cc37b546-915d-4795-9095-7471cdaeda28")
    );

    [Test]
    public async Task Constructor_should_throw_given_null_contestId_arg()
    {
        // Assert
        await Assert
            .That(() => _ = new ContestRole(null!, ArbitraryContestRoleType))
            .Throws<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'contestId')");
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_same_instance()
    {
        // Arrange
        ContestRole sut = new(ArbitraryContestId, ArbitraryContestRoleType);

        // Act
        bool result = sut.Equals(sut);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_true_when_other_is_ContestRole_with_equal_values()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sharedContestId, sharedContestRoleType);
        ContestRole other = new(sharedContestId, sharedContestRoleType);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_ContestRole_with_unequal_ContestId()
    {
        // Arrange
        ContestId sutContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        ContestId otherContestId = ContestId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));

        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sutContestId, sharedContestRoleType);
        ContestRole other = new(otherContestId, sharedContestRoleType);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_ContestRole_with_unequal_ContestRoleType()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sutContestRoleType = ContestRoleType.GlobalTelevote;
        const ContestRoleType otherContestRoleType = ContestRoleType.Participant;

        ContestRole sut = new(sharedContestId, sutContestRoleType);
        ContestRole other = new(sharedContestId, otherContestRoleType);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_not_ContestRole()
    {
        // Arrange
        ContestRole sut = new(ArbitraryContestId, ArbitraryContestRoleType);
        JuryAward other = new(ArbitraryCountryId, ArbitraryPointsValue);

        // Act
        bool result = sut.Equals(other);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equals_should_return_false_when_other_is_null()
    {
        // Arrange
        ContestRole sut = new(ArbitraryContestId, ArbitraryContestRoleType);

        // Act
        bool result = sut.Equals(null);

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_true_when_other_is_ContestRole_with_equal_values()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sharedContestId, sharedContestRoleType);
        ContestRole other = new(sharedContestId, sharedContestRoleType);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_ContestRole_with_unequal_ContestId()
    {
        // Arrange
        ContestId sutContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        ContestId otherContestId = ContestId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));

        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sutContestId, sharedContestRoleType);
        ContestRole other = new(otherContestId, sharedContestRoleType);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_ContestRole_with_unequal_ContestRoleType()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sutContestRoleType = ContestRoleType.GlobalTelevote;
        const ContestRoleType otherContestRoleType = ContestRoleType.Participant;

        ContestRole sut = new(sharedContestId, sutContestRoleType);
        ContestRole other = new(sharedContestId, otherContestRoleType);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Equality_operator_should_return_false_when_other_is_not_ContestRole()
    {
        // Arrange
        ContestRole sut = new(ArbitraryContestId, ArbitraryContestRoleType);
        JuryAward other = new(ArbitraryCountryId, ArbitraryPointsValue);

        // Act
        bool result = sut == other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_false_when_other_is_ContestRole_with_equal_values()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sharedContestId, sharedContestRoleType);
        ContestRole other = new(sharedContestId, sharedContestRoleType);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_ContestRole_with_unequal_ContestId()
    {
        // Arrange
        ContestId sutContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        ContestId otherContestId = ContestId.FromValue(Guid.Parse("cbafd594-551e-4d87-8089-a8450d4ad059"));

        const ContestRoleType sharedContestRoleType = ContestRoleType.GlobalTelevote;

        ContestRole sut = new(sutContestId, sharedContestRoleType);
        ContestRole other = new(otherContestId, sharedContestRoleType);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_ContestRole_with_unequal_ContestRoleType()
    {
        // Arrange
        ContestId sharedContestId = ContestId.FromValue(Guid.Parse("26786949-965a-44cc-801a-22c6b5667a3b"));
        const ContestRoleType sutContestRoleType = ContestRoleType.GlobalTelevote;
        const ContestRoleType otherContestRoleType = ContestRoleType.Participant;

        ContestRole sut = new(sharedContestId, sutContestRoleType);
        ContestRole other = new(sharedContestId, otherContestRoleType);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task Inequality_operator_should_return_true_when_other_is_not_ContestRole()
    {
        // Arrange
        ContestRole sut = new(ArbitraryContestId, ArbitraryContestRoleType);
        JuryAward other = new(ArbitraryCountryId, ArbitraryPointsValue);

        // Act
        bool result = sut != other;

        // Assert
        await Assert.That(result).IsTrue();
    }
}
