using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
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

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes().That().AreNotAbstract()
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
        TypeRule rule = Types().That().AreEnums()
            .Should().ResideInNamespaceMatching(".Enums");

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;
}
