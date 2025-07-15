using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Features.PublicApi.V0.Common.Contracts;
using Eurocentric.Features.Shared.Messaging;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "architecture")]
public sealed class FeaturesArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(VotingMethodFilter).Assembly)
        .Build();

    [Fact]
    public void Public_non_abstract_classes_should_not_be_nested() => Classes()
        .That().ArePublic()
        .And().AreNotAbstract()
        .Should().NotBeNested()
        .Check(Architecture);

    [Fact]
    public void Classes_that_are_not_abstract_should_be_sealed() => Classes()
        .That().AreNotAbstract()
        .Should().BeSealed()
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_IRequest_should_implement_ICommand_or_IQuery() => Classes()
        .That().ImplementInterface(typeof(IRequest<>))
        .Should().ImplementInterface(typeof(ICommand<>))
        .OrShould().ImplementInterface(typeof(IQuery<>))
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_ICommand_should_be_internal_sealed_nested_immutable_records_named_Command() => Classes()
        .That().ImplementInterface(typeof(ICommand<>))
        .Should().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().BeImmutable()
        .AndShould().BeRecord()
        .AndShould().HaveName("Command")
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_IQuery_should_be_internal_sealed_nested_immutable_records_named_Query() => Classes()
        .That().ImplementInterface(typeof(IQuery<>))
        .Should().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().BeImmutable()
        .AndShould().BeRecord()
        .AndShould().HaveName("Query")
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_IRequestHandler_should_implement_ICommandHandler_or_IQueryHandler() => Classes()
        .That().ImplementInterface(typeof(IRequestHandler<,>))
        .Should().ImplementInterface(typeof(ICommandHandler<,>))
        .OrShould().ImplementInterface(typeof(IQueryHandler<,>))
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_IRequestHandler_should_not_be_records() => Classes()
        .That().ImplementInterface(typeof(IRequestHandler<,>))
        .Should().NotBeRecord()
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_IQueryHandler_should_be_internal_sealed_nested_and_named_Handler() => Classes()
        .That().ImplementInterface(typeof(IQueryHandler<,>))
        .Should().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().HaveName("Handler")
        .Check(Architecture);

    [Fact]
    public void Classes_that_implement_ICommandHandler_should_be_internal_sealed_nested_and_named_Handler() => Classes()
        .That().ImplementInterface(typeof(ICommandHandler<,>))
        .Should().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().HaveName("Handler")
        .Check(Architecture);


    [Fact]
    public void Request_classes_should_be_public_sealed_non_nested_immutable_records() => Classes()
        .That().HaveNameEndingWith("Request")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeImmutable()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void Response_classes_should_be_public_sealed_non_nested_immutable_records() => Classes()
        .That().HaveNameEndingWith("Response")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeImmutable()
        .AndShould().BeRecord()
        .Check(Architecture);
}
