using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Loader;
using Eurocentric.Features.Shared.Messaging;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Category("architecture")]
public sealed class FeaturesArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(IQuery<>).Assembly)
        .Build();

    private static readonly IObjectProvider<IType> AdminApiV0NamespaceTypes = Types()
        .That().ResideInNamespaceMatching(@"Eurocentric\.Features\.AdminApi\.V0.*");

    private static readonly IObjectProvider<IType> AdminApiV1NamespaceTypes = Types()
        .That().ResideInNamespaceMatching(@"Eurocentric\.Features\.AdminApi\.V1.*");

    private static readonly IObjectProvider<IType> PublicApiV0NamespaceTypes = Types()
        .That().ResideInNamespaceMatching(@"Eurocentric\.Features\.PublicApi\.V0.*");

    [Test]
    public async Task Public_non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ArePublic()
            .And().AreNotAbstract()
            .Should().BeSealed();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_abstract_and_internal_and_not_nested()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().HaveNameEndingWith("Feature")
            .Should().BeAbstract()
            .AndShould().BeInternal()
            .AndShould().NotBeNested();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Request_classes_should_be_public_sealed_non_nested_immutable_records()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().HaveNameEndingWith("Request")
            .Should().BePublic()
            .AndShould().BeSealed()
            .AndShould().NotBeNested()
            .AndShould().BeRecord()
            .AndShould().BeImmutable();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Response_classes_should_be_public_sealed_non_nested_immutable_records()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().HaveNameEndingWith("Response")
            .Should().BePublic()
            .AndShould().BeSealed()
            .AndShould().NotBeNested()
            .AndShould().BeRecord()
            .AndShould().BeImmutable();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequest_should_implement_ICommand_or_IQuery()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().AreNot(typeof(ICommand<>))
            .And().AreNot(typeof(IQuery<>))
            .And().ImplementInterface(typeof(IRequest<>))
            .Should().ImplementInterface(typeof(ICommand<>))
            .OrShould().ImplementInterface(typeof(IQuery<>));

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Classes_that_implement_ICommand_should_be_nested_internal_sealed_immutable_records_named_Command()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ImplementInterface(typeof(ICommand<>))
            .Should().BeNested()
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().BeImmutable()
            .AndShould().BeRecord()
            .AndShould().HaveName("Command");

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Classes_that_implement_IQuery_should_be_nested_internal_sealed_immutable_records_named_Query()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ImplementInterface(typeof(IQuery<>))
            .Should().BeNested()
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().BeImmutable()
            .AndShould().BeRecord()
            .AndShould().HaveName("Query");

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequestHandler_should_implement_ICommandHandler_or_IQueryHandler()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().AreNot(typeof(ICommandHandler<,>))
            .And().AreNot(typeof(IQueryHandler<,>))
            .And().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().ImplementInterface(typeof(ICommandHandler<,>))
            .OrShould().ImplementInterface(typeof(IQueryHandler<,>));

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Classes_that_implement_ICommandHandler_should_be_nested_internal_sealed_classes_named_CommandHandler()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ImplementInterface(typeof(ICommandHandler<,>))
            .Should().BeNested()
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().HaveName("CommandHandler");

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Classes_that_implement_IQueryHandler_should_be_nested_internal_sealed_classes_named_QueryHandler()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ImplementInterface(typeof(IQueryHandler<,>))
            .Should().BeNested()
            .AndShould().BeInternal()
            .AndShould().BeSealed()
            .AndShould().HaveName("QueryHandler");

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_in_AdminApi_V0_namespace_should_not_depend_on_other_API_version_types()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().Are(AdminApiV0NamespaceTypes)
            .Should().NotDependOnAny(AdminApiV1NamespaceTypes)
            .AndShould().NotDependOnAny(PublicApiV0NamespaceTypes);

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_in_AdminApi_V1_namespace_should_not_depend_on_other_API_version_types()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().Are(AdminApiV1NamespaceTypes)
            .Should().NotDependOnAny(AdminApiV0NamespaceTypes)
            .AndShould().NotDependOnAny(PublicApiV0NamespaceTypes);

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Types_in_PublicApi_V0_namespace_should_not_depend_on_other_API_version_types()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().Are(PublicApiV0NamespaceTypes)
            .Should().NotDependOnAny(AdminApiV0NamespaceTypes)
            .AndShould().NotDependOnAny(AdminApiV1NamespaceTypes);

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }
}
