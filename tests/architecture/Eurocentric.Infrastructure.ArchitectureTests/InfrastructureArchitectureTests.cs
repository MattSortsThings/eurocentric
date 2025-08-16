using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Infrastructure.DataAccess.Dapper;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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

    private static readonly IObjectProvider<IType> ExpectedPublicTypes = Types()
        .That().ResideInNamespaceMatching("DataAccess.Common")
        .Or().Are(typeof(DependencyInjection), typeof(AppDbContext), typeof(IDbStoredProcedureRunner));

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .And().AreNot(TypesInMigrationsNamespace)
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_except_expected_public_types_should_not_be_public()
    {
        // Arrange
        TypeRule rule = Types()
            .That().AreNot(TypesInMigrationsNamespace)
            .And().AreNot(ExpectedPublicTypes)
            .Should().NotBePublic();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task SaveChangesInterceptor_classes_should_have_name_ending_with_SaveChangesInterceptor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreAssignableTo(typeof(SaveChangesInterceptor))
            .Should().HaveNameEndingWith("SaveChangesInterceptor");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;
}
