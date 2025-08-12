using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Loader;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Category("architecture")]
public sealed class InfrastructureArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(AppDbContext).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> MigrationsNamespaceClasses = Classes()
        .That().ResideInNamespaceMatching(@"\.Migrations");

    private static readonly IObjectProvider<IType> DataAccessPublicTypes = Types()
        .That().ResideInNamespaceMatching("Eurocentric.Infrastructure.DataAccess.Common.*")
        .Or().HaveName("AppDbContext")
        .Or().HaveName("IDbStoredProcedureRunner");

    [Test]
    public async Task Public_non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ArePublic()
            .And().AreNot(MigrationsNamespaceClasses)
            .And().AreNotAbstract()
            .Should().BeSealed();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_not_in_root_namespace_or_data_access_public_types_should_not_be_public()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().DoNotResideInNamespace("Eurocentric.Infrastructure")
            .And().AreNot(MigrationsNamespaceClasses)
            .And().AreNot(DataAccessPublicTypes)
            .Should().NotBePublic();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }
}
