using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Features.AdminApi.V0.Common.Dtos;
using Eurocentric.Features.Shared.Messaging;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "architecture")]
public sealed class FeaturesArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Contest).Assembly)
        .Build();

    private static readonly IObjectProvider<IType> AdminApiV0Types = Types()
        .That()
        .ResideInNamespace(@"Eurocentric\.Features\.AdminApi\.V0.*", true);

    private static readonly IObjectProvider<IType> AdminApiV1Types = Types()
        .That()
        .ResideInNamespace(@"Eurocentric\.Features\.AdminApi\.V1.*", true);

    private static readonly IObjectProvider<IType> PublicApiV0Types = Types()
        .That()
        .ResideInNamespace(@"Eurocentric\.Features\.PublicApi\.V0.*", true);

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
    public void Admin_API_V0_types_should_depend_on_no_other_API_major_version_types() => Types()
        .That().Are(AdminApiV0Types)
        .Should().NotDependOnAny(AdminApiV1Types)
        .AndShould().NotDependOnAny(PublicApiV0Types)
        .Check(Architecture);

    [Fact]
    public void Admin_API_V1_types_should_depend_on_no_other_API_major_version_types() => Types()
        .That().Are(AdminApiV1Types)
        .Should().NotDependOnAny(AdminApiV0Types)
        .AndShould().NotDependOnAny(PublicApiV0Types)
        .Check(Architecture);

    [Fact]
    public void Public_API_V0_types_should_depend_on_no_other_API_major_version_types() => Types()
        .That().Are(PublicApiV0Types)
        .Should().NotDependOnAny(AdminApiV0Types)
        .AndShould().NotDependOnAny(AdminApiV1Types)
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

    [Fact]
    public void QueryParams_classes_should_be_public_sealed_non_nested_immutable_records() => Classes()
        .That().HaveNameEndingWith("QueryParams")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeImmutable()
        .AndShould().BeRecord()
        .Check(Architecture);
}
