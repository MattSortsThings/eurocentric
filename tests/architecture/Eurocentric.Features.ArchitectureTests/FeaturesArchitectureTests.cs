using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Features.Shared.Messaging;
using Microsoft.AspNetCore.OpenApi;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "architecture")]
public class FeaturesArchitectureTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Middleware).Assembly)
        .Build();

    [Fact]
    public void Classes_that_are_not_abstract_should_be_sealed() => Classes()
        .That().AreNotAbstract()
        .Should().BeSealed()
        .Check(Architecture);

    [Fact]
    public void Request_classes_should_be_public_sealed_non_nested_records() => Classes()
        .That().HaveNameEndingWith("Request")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void Response_classes_should_be_public_sealed_non_nested_records() => Classes()
        .That().HaveNameEndingWith("Response")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void QueryParams_classes_should_be_public_sealed_non_nested_records() => Classes()
        .That().HaveNameEndingWith("QueryParams")
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void ICommand_implementations_should_have_name_Command_and_be_internal_sealed_nested_records() => Classes()
        .That().ImplementInterface(typeof(ICommand<>))
        .Should().HaveName("Command")
        .AndShould().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void ICommandHandler_implementations_should_have_name_Handler_and_be_internal_sealed_nested_classes() => Classes()
        .That()
        .ImplementInterface(typeof(ICommandHandler<,>))
        .Should().HaveName("Handler")
        .AndShould().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .Check(Architecture);

    [Fact]
    public void IQuery_implementations_should_have_name_Query_and_be_internal_sealed_nested_records() => Classes()
        .That().ImplementInterface(typeof(IQuery<>))
        .Should().HaveName("Query")
        .AndShould().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void IQueryHandler_implementations_should_have_name_Handler_and_be_internal_sealed_nested_classes() => Classes()
        .That().ImplementInterface(typeof(IQueryHandler<,>))
        .Should().HaveName("Handler")
        .AndShould().BeInternal()
        .AndShould().BeSealed()
        .AndShould().BeNested()
        .Check(Architecture);

    [Fact]
    public void IRequest_implementations_should_implement_ICommand_or_implement_IQuery() => Classes()
        .That().ImplementInterface(typeof(IRequest<>))
        .Should().ImplementInterface(typeof(ICommand<>))
        .OrShould().ImplementInterface(typeof(IQuery<>))
        .Check(Architecture);

    [Fact]
    public void IRequestHandler_implementations_should_implement_ICommandHandler_or_implement_IQueryHandler() => Classes()
        .That().ImplementInterface(typeof(IRequestHandler<,>))
        .Should().ImplementInterface(typeof(ICommandHandler<,>))
        .OrShould().ImplementInterface(typeof(IQueryHandler<,>))
        .Check(Architecture);

    [Fact]
    public void Endpoint_classes_should_be_private_nested_classes() => Classes()
        .That().HaveName("Endpoint")
        .Should().BePrivate()
        .AndShould().BeNested()
        .Check(Architecture);

    [Fact]
    public void IOpenApiDocumentTransformer_implementations_should_be_internal_with_name_ending_in_DocumentTransformer() =>
        Classes()
            .That().ImplementInterface(typeof(IOpenApiDocumentTransformer))
            .Should().BeInternal()
            .AndShould().HaveNameEndingWith("DocumentTransformer")
            .Check(Architecture);

    [Fact]
    public void IOpenApiOperationTransformer_implementations_should_be_internal_with_name_ending_in_OperationTransformer() =>
        Classes()
            .That().ImplementInterface(typeof(IOpenApiOperationTransformer))
            .Should().BeInternal()
            .AndShould().HaveNameEndingWith("OperationTransformer")
            .Check(Architecture);

    [Fact]
    public void IOpenApiSchemaTransformer_implementations_should_be_internal_with_name_ending_in_SchemaTransformer() =>
        Classes()
            .That().ImplementInterface(typeof(IOpenApiSchemaTransformer))
            .Should().BeInternal()
            .AndShould().HaveNameEndingWith("SchemaTransformer")
            .Check(Architecture);
}
