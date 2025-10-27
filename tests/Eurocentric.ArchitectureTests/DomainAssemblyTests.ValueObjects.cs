using Eurocentric.ArchitectureTests.TestUtils;
using Eurocentric.Domain.Core;

namespace Eurocentric.ArchitectureTests;

public sealed partial class DomainAssemblyTests
{
    private static readonly IObjectProvider<IType> ValueObjectTypes = Types()
        .That()
        .Are(DomainAssemblyTypes)
        .And()
        .AreAssignableTo(typeof(ValueObject))
        .And()
        .DoNotResideInNamespace("Eurocentric.Domain.Core");

    private static readonly IObjectProvider<IType> AtomicValueObjectTypes = Types()
        .That()
        .Are(ValueObjectTypes)
        .And()
        .AreAssignableTo(
            typeof(DateOnlyAtomicValueObject),
            typeof(GuidAtomicValueObject),
            typeof(Int32AtomicValueObject),
            typeof(StringAtomicValueObject)
        );

    private static readonly IObjectProvider<IType> CompoundValueObjectTypes = Types()
        .That()
        .Are(ValueObjectTypes)
        .And()
        .AreAssignableTo(typeof(CompoundValueObject));

    [Test]
    public async Task ValueObject_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(ValueObjectTypes).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_reside_in_ValueObjects_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(ValueObjectTypes)
            .Should()
            .ResideInNamespace("Eurocentric.Domain.ValueObjects");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_be_immutable()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(ValueObjectTypes).Should().BeImmutable();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task AtomicValueObject_classes_should_have_no_public_constructor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(AtomicValueObjectTypes)
            .Should()
            .FollowCustomCondition(new ClassHasNoPublicConstructorCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task CompoundValueObject_classes_should_have_public_constructor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(CompoundValueObjectTypes)
            .Should()
            .FollowCustomCondition(new ClassHasPublicConstructorCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
