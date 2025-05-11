using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Domain.Enums;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Domain.ArchitectureTests;

[Trait("Category", "architecture")]
public class DomainArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(ContestFormat).Assembly)
        .Build();

    [Fact]
    public void Enum_types_should_be_public() => Types()
        .That().AreEnums()
        .Should()
        .BePublic()
        .Check(Architecture);
}
