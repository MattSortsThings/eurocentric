using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Domain.Abstractions;
using Eurocentric.Domain.Enums;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;

namespace Eurocentric.Domain.ArchitectureTests;

[Category("architecture")]
public sealed class DomainArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(PointsValue).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> ValueObjectClasses = Classes()
        .That().AreAssignableTo(typeof(ValueObject))
        .And().DoNotResideInNamespace("Eurocentric.Domain.Abstractions");

    private static readonly IObjectProvider<Class> EntityClasses = Classes()
        .That().AreAssignableTo(typeof(Entity))
        .And().DoNotResideInNamespace("Eurocentric.Domain.Abstractions");

    private static readonly IObjectProvider<Class> AggregateBuilderClasses = Classes()
        .That().ResideInNamespaceMatching("Eurocentric.Domain.Aggregates")
        .And().HaveNameEndingWith("Builder");

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Enums_should_reside_in_Enums_namespace()
    {
        // Arrange
        TypeRule rule = Types()
            .That().AreEnums()
            .Should().ResideInNamespace("Eurocentric.Domain.Enums");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_reside_in_ValueObjects_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().ResideInNamespace("Eurocentric.Domain.ValueObjects");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_have_no_constructor_accessible_from_outside_assembly()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ValueObjectClasses)
            .Should().FollowCustomCondition(HasNoConstructorAccessibleFromOutsideAssembly,
                "has no public constructors",
                "at least one public constructor was found");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_have_no_constructor_accessible_from_outside_assembly()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .Should().FollowCustomCondition(HasNoConstructorAccessibleFromOutsideAssembly,
                "has no public constructors",
                "at least one public constructor was found");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_have_private_or_private_protected_parameterless_constructor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(EntityClasses)
            .Should().FollowCustomCondition(HasPrivateOrPrivateProtectedParameterlessConstructor,
                "has private or private protected parameterless constructor",
                "no private or private protected parameterless constructor was found");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Aggregate_builder_classes_should_have_no_constructor_accessible_from_outside_assembly()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(AggregateBuilderClasses)
            .Should().FollowCustomCondition(HasNoConstructorAccessibleFromOutsideAssembly,
                "has no public constructors",
                "at least one public constructor was found");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;

    private static bool HasNoConstructorAccessibleFromOutsideAssembly(Class @class) =>
        @class.Constructors.All(member =>
            member.Visibility is Visibility.Private or Visibility.PrivateProtected or Visibility.Internal);

    private static bool HasPrivateOrPrivateProtectedParameterlessConstructor(Class @class) =>
        @class.Constructors.Any(member => member.Visibility is Visibility.Private or Visibility.PrivateProtected
                                          && !member.Parameters.Any());
}
