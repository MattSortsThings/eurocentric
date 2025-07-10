using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Infrastructure.DataAccess.EfCore;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Trait("Category", "architecture")]
public sealed class InfrastructureArchitectureTests
{
    private const string RootNamespace = "Eurocentric.Infrastructure";
    private const string MigrationsNamespace = "Eurocentric.Infrastructure.DataAccess.EfCore.Migrations";

    private static readonly IObjectProvider<IType> ExpectedPublicTypes = Types()
        .That()
        .HaveName("AppDbContext")
        .Or().HaveName("IDbStoredProcedureRunner")
        .Or().ResideInNamespace("Eurocentric.Infrastructure.DataAccess.Constants")
        .Or().ResideInNamespace("Eurocentric.Infrastructure.DataAccess.EfCore.Migrations");

    private static readonly IObjectProvider<IType> GeneratedMigrationTypes = Types()
        .That().ResideInNamespace(MigrationsNamespace);

    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(AppDbContext).Assembly)
        .Build();

    [Fact]
    public void Public_non_abstract_classes_should_not_be_nested() => Classes()
        .That().ArePublic()
        .And().AreNotAbstract()
        .And().AreNot(GeneratedMigrationTypes)
        .Should().NotBeNested()
        .Check(Architecture);

    [Fact]
    public void Classes_that_are_not_abstract_should_be_sealed() => Classes()
        .That().AreNotAbstract()
        .And().AreNot(GeneratedMigrationTypes)
        .Should().BeSealed()
        .Check(Architecture);

    [Fact]
    public void Types_not_in_root_namespace_should_not_be_public() => Types()
        .That().DoNotResideInNamespace(RootNamespace)
        .And().AreNot(GeneratedMigrationTypes)
        .And().AreNot(ExpectedPublicTypes)
        .Should()
        .NotBePublic()
        .Check(Architecture);
}
