using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Features.AdminApi.V0.Contests.Models;
using Eurocentric.Features.Shared.Messaging;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class FeaturesArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Contest).Assembly)
        .Build();

    [Fact]
    public void Classes_with_name_ending_with_Command_should_be_public_records_that_implement_generic_ICommand() => Classes()
        .That()
        .HaveNameEndingWith("Command")
        .Should().BePublic().AndShould().BeRecord().AndShould().ImplementInterface(typeof(ICommand<>))
        .Check(Architecture);

    [Fact]
    public void Classes_with_name_ending_with_Query_should_be_public_records_that_implement_generic_IQuery() => Classes()
        .That()
        .HaveNameEndingWith("Query")
        .Should().BePublic().AndShould().BeRecord().AndShould().ImplementInterface(typeof(IQuery<>))
        .Check(Architecture);
}
