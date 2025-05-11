using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Infrastructure.FakeRepositories;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Trait("Category", "architecture")]
public class InfrastructureArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(FakeVoterPointsDataRepository).Assembly)
        .Build();

    [Fact]
    public void Public_non_abstract_classes_should_be_sealed() => Classes()
        .That().ArePublic()
        .And().AreNotAbstract()
        .Should()
        .BeSealed()
        .Check(Architecture);
}
