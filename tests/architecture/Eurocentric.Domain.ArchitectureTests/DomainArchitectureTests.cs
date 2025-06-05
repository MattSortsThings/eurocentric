using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Domain.Enums;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Domain.ArchitectureTests;

[Trait("Category", "architecture")]
public sealed class DomainArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(ContestFormat).Assembly)
        .Build();

    [Fact]
    public void Public_types_should_not_be_nested() => Types()
        .That().ArePublic()
        .Should().NotBeNested()
        .Check(Architecture);

    [Fact]
    public void Classes_that_are_not_abstract_should_be_sealed() => Classes()
        .That().AreNotAbstract()
        .Should().BeSealed()
        .Check(Architecture);
}
