using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Features.Shared.Messaging;
using Eurocentric.Features.Shared.Security;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;

namespace Eurocentric.Features.ArchitectureTests;

[Category("architecture")]
public sealed class FeaturesArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(IQuery<>).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> RequestClasses = Classes()
        .That().HaveNameEndingWith("Request")
        .And().DoNotResideInNamespaceMatching("Common");

    private static readonly IObjectProvider<Class> ResponseClasses = Classes()
        .That().HaveNameEndingWith("Response")
        .And().DoNotResideInNamespaceMatching("Common");

    private static readonly IObjectProvider<Class> FeatureClasses = Classes()
        .That().HaveNameEndingWith("Feature");

    private static readonly IObjectProvider<Class> CommandClasses = Classes()
        .That().ImplementInterface(typeof(ICommand<>));

    private static readonly IObjectProvider<Class> QueryClasses = Classes()
        .That().ImplementInterface(typeof(IQuery<>));

    private static readonly IObjectProvider<Class> CommandHandlerClasses = Classes()
        .That().ImplementInterface(typeof(ICommandHandler<,>));

    private static readonly IObjectProvider<Class> QueryHandlerClasses = Classes()
        .That().ImplementInterface(typeof(IQueryHandler<,>));

    private static readonly IObjectProvider<Class> DomainEventHandlerClasses = Classes()
        .That().ImplementInterface(typeof(IDomainEventHandler<>));

    private static readonly IObjectProvider<IType> AdminApiTypes = Types()
        .That().ResideInNamespaceMatching(".AdminApi");

    private static readonly IObjectProvider<IType> AdminApiV1Types = Types()
        .That().ResideInNamespaceMatching(".AdminApi.V1");

    private static readonly IObjectProvider<IType> PublicApiTypes = Types()
        .That().ResideInNamespaceMatching(".PublicApi");

    private static readonly IObjectProvider<IType> PublicApiV1Types = Types()
        .That().ResideInNamespaceMatching(".PublicApi.V1");

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Public_types_should_not_be_nested()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ArePublic()
            .Should().NotBeNested();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Classes_that_implement_IRequest_should_implement_ICommand_or_IQuery()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().ImplementInterface(typeof(IRequest<>))
            .Should().ImplementInterface(typeof(ICommand<>))
            .OrShould().ImplementInterface(typeof(IQuery<>));

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Classes_that_implement_IRequestHandler_should_implement_ICommandHandler_or_IQueryHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().ImplementInterface(typeof(IRequestHandler<,>))
            .And().AreNot(typeof(ICommandHandler<,>), typeof(IQueryHandler<,>))
            .Should().ImplementInterface(typeof(ICommandHandler<,>))
            .OrShould().ImplementInterface(typeof(IQueryHandler<,>));

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Classes_that_implement_IConsumer_should_implement_IDomainEventHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().ImplementInterface(typeof(IConsumer<>))
            .And().AreNot(typeof(IDomainEventHandler<>))
            .Should().ImplementInterface(typeof(IDomainEventHandler<>));

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_have_name_Command()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandClasses)
            .Should().HaveName("Command");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_not_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandClasses)
            .Should().NotBeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_nested_in_Feature_class()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandClasses)
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_have_name_Query()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryClasses)
            .Should().HaveName("Query");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_not_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryClasses)
            .Should().NotBeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Query_classes_should_be_nested_in_Feature_class()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryClasses)
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandHandlerClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandHandlerClasses)
            .Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_not_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandHandlerClasses)
            .Should().NotBeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_have_name_CommandHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandHandlerClasses)
            .Should().HaveName("CommandHandler");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_be_nested_in_Feature_class()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(CommandHandlerClasses)
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryHandlerClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryHandlerClasses)
            .Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_not_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryHandlerClasses)
            .Should().NotBeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_have_name_QueryHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryHandlerClasses)
            .Should().HaveName("QueryHandler");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task QueryHandler_classes_should_be_nested_in_Feature_class()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(QueryHandlerClasses)
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventHandlerClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_not_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventHandlerClasses)
            .Should().NotBeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventHandlerClasses)
            .Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_have_name_DomainEventHandler()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventHandlerClasses)
            .Should().HaveName("DomainEventHandler");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEventHandler_classes_should_be_nested_in_Feature_class()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DomainEventHandlerClasses)
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(RequestClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_classes_should_have_name_equal_to_namespace_last_segment_plus_Request()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(RequestClasses)
            .Should().FollowCustomCondition(cls =>
                    cls.Name == cls.Namespace.Name.Split('.').Last() + "Request",
                "have name equal to namespace last segment + 'Request'",
                "name does not match");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Request_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(RequestClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Response_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ResponseClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Response_classes_should_have_name_equal_to_namespace_last_segment_plus_Response()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ResponseClasses)
            .Should().FollowCustomCondition(cls =>
                    cls.Name == cls.Namespace.Name.Split('.').Last() + "Response",
                "have name equal to namespace last segment + 'Response'",
                "name does not match");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Response_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(ResponseClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureClasses)
            .Should().BeAbstract();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_have_name_equal_to_namespace_last_segment_plus_Feature()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureClasses)
            .Should().FollowCustomCondition(cls =>
                    cls.Name == cls.Namespace.Name.Split('.').Last() + "Feature",
                "have name equal to namespace last segment + 'Feature'",
                "name does not match");

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task AdminApi_types_should_not_depend_on_PublicApi_types()
    {
        // Arrange
        TypeRule rule = Types()
            .That().Are(AdminApiTypes)
            .Should().NotDependOnAny(PublicApiTypes);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task PublicApi_types_should_not_depend_on_AdminApi_types()
    {
        // Arrange
        TypeRule rule = Types()
            .That().Are(AdminApiTypes)
            .Should().NotDependOnAny(PublicApiTypes);

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    [Arguments("AdminApi.V1.Broadcasts.AwardJuryPoints")]
    [Arguments("AdminApi.V1.Broadcasts.AwardTelevotePoints")]
    [Arguments("AdminApi.V1.Broadcasts.DeleteBroadcast")]
    [Arguments("AdminApi.V1.Broadcasts.DisqualifyCompetitor")]
    [Arguments("AdminApi.V1.Broadcasts.GetBroadcast")]
    [Arguments("AdminApi.V1.Broadcasts.GetBroadcasts")]
    [Arguments("AdminApi.V1.Contests.CreateChildBroadcast")]
    [Arguments("AdminApi.V1.Contests.CreateContest")]
    [Arguments("AdminApi.V1.Contests.DeleteContest")]
    [Arguments("AdminApi.V1.Contests.GetContest")]
    [Arguments("AdminApi.V1.Contests.HandleBroadcastCompleted")]
    [Arguments("AdminApi.V1.Contests.HandleBroadcastDeleted")]
    [Arguments("AdminApi.V1.Countries.CreateCountry")]
    [Arguments("AdminApi.V1.Countries.DeleteCountry")]
    [Arguments("AdminApi.V1.Countries.GetCountries")]
    [Arguments("AdminApi.V1.Countries.GetCountry")]
    [Arguments("AdminApi.V1.Countries.HandleContestCreated")]
    [Arguments("AdminApi.V1.Countries.HandleContestDeleted")]
    public async Task AdminApi_V1_feature_types_should_not_depend_on_any_other_AdminApi_V1_feature_namespace(
        string featureNamespace)
    {
        // Arrange
        TypeRule rule = Types()
            .That().ResideInNamespaceMatching(featureNamespace)
            .Should().NotDependOnAny(Types()
                .That().Are(AdminApiV1Types)
                .And().DoNotResideInNamespaceMatching(featureNamespace)
                .And().DoNotResideInNamespaceMatching(".Common"));

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    [Arguments("PublicApi.V1.Queryables.GetQueryableBroadcasts")]
    [Arguments("PublicApi.V1.Queryables.GetQueryableContests")]
    [Arguments("PublicApi.V1.Queryables.GetQueryableContestStages")]
    [Arguments("PublicApi.V1.Queryables.GetQueryableCountries")]
    [Arguments("PublicApi.V1.Queryables.GetQueryableVotingMethods")]
    [Arguments("PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsAverageRankings")]
    [Arguments("PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsConsensusRankings")]
    [Arguments("PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsInRangeRankings")]
    [Arguments("PublicApi.V1.Rankings.CompetingCountries.GetCompetingCountryPointsShareRankings")]
    [Arguments("PublicApi.V1.Rankings.Competitors.GetCompetitorPointsAverageRankings")]
    [Arguments("PublicApi.V1.Rankings.Competitors.GetCompetitorPointsConsensusRankings")]
    [Arguments("PublicApi.V1.Rankings.Competitors.GetCompetitorPointsInRangeRankings")]
    [Arguments("PublicApi.V1.Rankings.Competitors.GetCompetitorPointsShareRankings")]
    [Arguments("PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsAverageRankings")]
    [Arguments("PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsConsensusRankings")]
    [Arguments("PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsInRangeRankings")]
    [Arguments("PublicApi.V1.Rankings.VotingCountries.GetVotingCountryPointsShareRankings")]
    public async Task PublicApi_V1_feature_types_should_not_depend_on_any_other_PublicApi_feature_namespace(
        string featureNamespace)
    {
        // Arrange
        TypeRule rule = Types()
            .That().ResideInNamespaceMatching(featureNamespace)
            .Should().NotDependOnAny(Types()
                .That().Are(PublicApiV1Types)
                .And().DoNotResideInNamespaceMatching(featureNamespace)
                .And().DoNotResideInNamespaceMatching(".Common"));

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    [Test]
    public async Task Shared_types_should_not_be_public()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ResideInNamespaceMatching("Shared")
            .And().AreNot(typeof(ApiKeyConstants), typeof(ApiKeySecurityOptions))
            .Should().NotBePublic();

        // Act
        IEnumerable<EvaluationResult> evaluation = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(evaluation).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;
}
