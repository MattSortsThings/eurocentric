using Eurocentric.ArchitectureTests.TestUtils;
using Eurocentric.Domain.Core;

namespace Eurocentric.ArchitectureTests;

public sealed partial class DomainAssemblyTests
{
    private static readonly IObjectProvider<IType> EntityTypes = Types()
        .That()
        .AreAssignableTo(typeof(Entity))
        .And()
        .DoNotResideInNamespace("Eurocentric.Domain.Core");

    private static readonly IObjectProvider<IType> AggregateRootTypes = Types()
        .That()
        .Are(EntityTypes)
        .And()
        .AreAssignableTo(typeof(AggregateRoot<>));

    private static readonly IObjectProvider<IType> OwnedEntityTypes = Types()
        .That()
        .Are(EntityTypes)
        .And()
        .AreNot(AggregateRootTypes);

    [Test]
    public async Task Entity_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EntityTypes).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_reside_in_Aggregates_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(EntityTypes)
            .Should()
            .ResideInNamespaceMatching("Eurocentric.Domain.Aggregates.");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_have_no_public_constructor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(EntityTypes)
            .Should()
            .FollowCustomCondition(new ClassHasNoPublicConstructorCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_have_no_public_property_setters()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(EntityTypes)
            .Should()
            .FollowCustomCondition(new ClassHasNoPublicPropertySettersCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task OwnedEntity_classes_should_have_no_public_methods()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(OwnedEntityTypes)
            .Should()
            .FollowCustomCondition(new ClassHasNoPublicMethodsCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
