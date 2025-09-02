using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Category("architecture")]
public sealed class InfrastructureArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(AppDbContext).Assembly)
        .Build();

    private static readonly IObjectProvider<IType> TypesInMigrationsNamespace = Types()
        .That().ResideInNamespaceMatching("DataAccess.EfCore.Migrations");

    private static readonly IObjectProvider<IType> ExplicitPublicTypes = Types()
        .That().ResideInNamespace("Eurocentric.Infrastructure")
        .Or().Are(typeof(AppDbContext));

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes().That().AreNotAbstract()
            .And().AreNot(TypesInMigrationsNamespace)
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_should_not_be_public()
    {
        // Arrange
        TypeRule rule = Types().That()
            .AreNot(TypesInMigrationsNamespace)
            .And().AreNot(ExplicitPublicTypes)
            .Should().NotBePublic();

        // Act
        IEnumerable<EvaluationResult> evaluationResult = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluationResult).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;
}
