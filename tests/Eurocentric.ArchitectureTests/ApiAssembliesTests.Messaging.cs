using Eurocentric.ArchitectureTests.TestUtils;
using Eurocentric.Domain.Core;
using SlimMessageBus;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests : ArchitectureTest
{
    private static readonly IObjectProvider<IType> TypesThatImplementIRequest = Types()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ImplementInterface(typeof(IRequest<>));

    private static readonly IObjectProvider<Class> CommandClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequest)
        .And()
        .ImplementInterface(typeof(ICommand<>));

    private static readonly IObjectProvider<Class> QueryClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequest)
        .And()
        .ImplementInterface(typeof(IQuery<>));

    private static readonly IObjectProvider<Class> UnitCommandClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequest)
        .And()
        .ImplementInterface(typeof(IUnitCommand));

    private static readonly IObjectProvider<IType> TypesThatImplementIRequestHandler = Types()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ImplementInterface(typeof(IRequestHandler<,>));

    private static readonly IObjectProvider<Class> CommandHandlerClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequestHandler)
        .And()
        .ImplementInterface(typeof(ICommandHandler<,>));

    private static readonly IObjectProvider<Class> QueryHandlerClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequestHandler)
        .And()
        .ImplementInterface(typeof(IQueryHandler<,>));

    private static readonly IObjectProvider<Class> UnitCommandHandlerClasses = Classes()
        .That()
        .Are(TypesThatImplementIRequestHandler)
        .And()
        .ImplementInterface(typeof(IUnitCommandHandler<>));

    private static readonly IObjectProvider<IType> TypesThatImplementIConsumer = Types()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ImplementInterface(typeof(IConsumer<>));

    private static readonly IObjectProvider<Class> DomainEventHandlerClasses = Classes()
        .That()
        .Are(TypesThatImplementIConsumer)
        .And()
        .ImplementInterface(typeof(IDomainEventHandler<>));

    [Test]
    public async Task Request_implementations_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequest).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_implementations_should_be_nested_in_Feature_classes()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequest).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_implementations_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequest).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_implementations_should_be_Command_Query_or_UnitCommand()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(TypesThatImplementIRequest)
            .Should()
            .Be(CommandClasses)
            .OrShould()
            .Be(QueryClasses)
            .OrShould()
            .Be(UnitCommandClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_have_name_Command()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(CommandClasses).Should().HaveName("Command");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_have_name_Query()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(QueryClasses).Should().HaveName("Query");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task UnitCommand_classes_should_have_name_UnitCommand()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(UnitCommandClasses).Should().HaveName("UnitCommand");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task RequestHandler_implementations_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequestHandler).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task RequestHandler_implementations_should_be_nested_in_Feature_classes()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequestHandler).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task RequestHandler_implementations_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementIRequestHandler).Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task RequestHandler_implementations_should_be_CommandHandler_QueryHandler_or_UnitCommandHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(TypesThatImplementIRequestHandler)
            .Should()
            .Be(CommandHandlerClasses)
            .OrShould()
            .Be(QueryHandlerClasses)
            .OrShould()
            .Be(UnitCommandHandlerClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_have_name_CommandHandler()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(CommandHandlerClasses).Should().HaveName("CommandHandler");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_have_name_QueryHandler()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(QueryHandlerClasses).Should().HaveName("QueryHandler");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task UnitCommandHandler_classes_should_have_name_UnitCommandHandler()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(UnitCommandHandlerClasses).Should().HaveName("UnitCommandHandler");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Consumer_implementations_should_be_internal()
    {
        // Arrange
        TypeRule rule = Types().That().Are(TypesThatImplementIConsumer).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Consumer_implementations_should_be_nested_in_Feature_classes()
    {
        // Arrange
        TypeRule rule = Types().That().Are(TypesThatImplementIConsumer).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Consumer_implementations_should_be_DomainEventHandler_classes()
    {
        // Arrange
        TypeRule rule = Types().That().Are(TypesThatImplementIConsumer).Should().Be(DomainEventHandlerClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventHandlerClasses).Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_have_name_DomainEventHandler()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventHandlerClasses).Should().HaveName("DomainEventHandler");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
