using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class InfrastructureArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(DependencyInjection).Assembly)
        .Build();

    [Fact]
    [Trait("Category", "Placeholder")]
    public void Should_always_pass() => Types()
        .That().HaveName("DependencyInjection")
        .Should().BePublic()
        .Check(Architecture);
}
