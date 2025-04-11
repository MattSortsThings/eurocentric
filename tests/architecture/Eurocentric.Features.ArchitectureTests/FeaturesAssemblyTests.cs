using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using Eurocentric.Features.Shared.Messaging;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class FeaturesAssemblyTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Middleware).Assembly)
        .Build();

    private static readonly IObjectProvider<IType> TypesInAdminApiNamespace = Types().That()
        .FollowCustomPredicate(t => t.Namespace.NameContains("AdminApi"), "Namespace contains AdminApi");

    private static readonly IObjectProvider<IType> TypesInPublicApiNamespace = Types().That()
        .FollowCustomPredicate(t => t.Namespace.NameContains("PublicApi"), "Namespace contains PublicApi");

    private static readonly IObjectProvider<Class> ResponseClasses = Classes().That().HaveNameEndingWith("Response");

    private static readonly IObjectProvider<Class> CommandClasses = Classes().That().HaveNameEndingWith("Command");

    private static readonly IObjectProvider<Class> QueryClasses = Classes().That().HaveNameEndingWith("Query");

    private static readonly IObjectProvider<Class> CommandHandlerClasses = Classes().That().HaveNameEndingWith("CommandHandler");

    private static readonly IObjectProvider<Class> QueryHandlerClasses = Classes().That().HaveNameEndingWith("QueryHandler");

    [Fact]
    public void Types_in_AdminApi_namespace_should_not_depend_on_types_in_PublicApi_namespace() => Types()
        .That().Are(TypesInAdminApiNamespace)
        .Should().NotDependOnAnyTypesThat().Are(TypesInPublicApiNamespace)
        .Check(Architecture);

    [Fact]
    public void Types_in_PublicApi_namespace_should_not_depend_on_types_in_AdminApi_namespace() => Types()
        .That().Are(TypesInPublicApiNamespace)
        .Should().NotDependOnAnyTypesThat().Are(TypesInAdminApiNamespace)
        .Check(Architecture);

    [Fact]
    public void Response_classes_should_be_public_sealed_non_nested_records() => Classes()
        .That().Are(ResponseClasses)
        .Should().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .AndShould().BeRecord()
        .Check(Architecture);

    [Fact]
    public void Command_classes_should_extend_generic_Request_class_and_be_public_sealed_and_not_nested() => Classes()
        .That().Are(CommandClasses)
        .Should().BeAssignableTo(typeof(Request<>))
        .AndShould().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .Check(Architecture);

    [Fact]
    public void Query_classes_should_extend_generic_Request_class_and_be_public_sealed_and_not_nested() => Classes()
        .That().Are(QueryClasses)
        .Should().BeAssignableTo(typeof(Request<>))
        .AndShould().BePublic()
        .AndShould().BeSealed()
        .AndShould().NotBeNested()
        .Check(Architecture);

    [Fact]
    public void CommandHandler_classes_should_extend_generic_RequestHandler_class_and_be_internal_sealed_and_not_nested() =>
        Classes()
            .That().Are(CommandHandlerClasses)
            .Should().BeAssignableTo(typeof(RequestHandler<,>))
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().NotBeNested()
            .Check(Architecture);

    [Fact]
    public void QueryHandler_classes_should_extend_generic_RequestHandler_class_and_be_internal_sealed_and_not_nested() =>
        Classes()
            .That().Are(QueryHandlerClasses)
            .Should().BeAssignableTo(typeof(RequestHandler<,>))
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().NotBeNested()
            .Check(Architecture);
}
