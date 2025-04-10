using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Domain.Placeholders;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Domain.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class DomainAssemblyTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Line).Assembly)
        .Build();

    [Fact]
    [Trait("Category", "Placeholder")]
    public void Should_always_pass() => Types().That().AreEnums().Should().HaveName("Line").Check(Architecture);
}
