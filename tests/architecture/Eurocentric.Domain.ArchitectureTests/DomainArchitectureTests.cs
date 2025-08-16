using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Domain.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;

namespace Eurocentric.Domain.ArchitectureTests;

[Category("architecture")]
public sealed class DomainArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(ValueObject).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> EntityClasses = Classes()
        .That().AreAssignableTo(typeof(Entity))
        .And().AreNot(typeof(AggregateRoot<>), typeof(Entity));

    private static readonly IObjectProvider<Class> ValueObjectClasses = Classes()
        .That().AreAssignableTo(typeof(ValueObject))
        .And().AreNot(typeof(ValueObject));

    private static readonly IObjectProvider<Class> DomainEventClasses = Classes()
        .That().ImplementInterface(typeof(IDomainEvent));

    [Test]
    public async Task Public_types_should_not_be_nested()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ArePublic()
            .Should().NotBeNested();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .Should().BePublic();

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
            .That().Are(EntityClasses)
            .Should().ResideInNamespaceMatching(".Aggregates");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_that_are_not_AggregateRoot_classes_should_not_have_Id_property()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .And().AreNotAssignableTo(typeof(AggregateRoot<>))
            .Should()
            .NotHavePropertyMemberWithName("Id");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_constructors_should_be_internal_or_private_protected_or_private()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .Should().FollowCustomCondition(AllConstructorsAreInternalOrPrivateProtectedOrPrivate,
                "all constructors should be internal or private protected or private",
                "at least one constructor has a wider visibility");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_be_immutable()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().BeImmutable();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_reside_in_Identifiers_or_ValueObjects_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().ResideInNamespaceMatching(".Identifiers")
            .OrShould().ResideInNamespaceMatching(".ValueObjects");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_have_name_ending_with_Event()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventClasses)
            .Should().HaveNameEndingWith("Event");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_reside_in_Events_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventClasses)
            .Should().ResideInNamespaceMatching(".Events");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    private static bool AllConstructorsAreInternalOrPrivateProtectedOrPrivate(Class cls) =>
        cls.Constructors.All(member =>
            member.Visibility is Visibility.Internal or Visibility.PrivateProtected or Visibility.Private);

    private static bool Passed(EvaluationResult result) => result.Passed;
}
