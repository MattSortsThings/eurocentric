using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class InfrastructureAssemblyTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(DependencyInjection).Assembly)
        .Build();

    [Fact]
    [Trait("Category", "Placeholder")]
    public void Should_always_pass() => Classes().Should().HaveName("DependencyInjection").Check(Architecture);
}
