using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Fluent.Conditions;
using ArchUnitNET.Loader;
using Eurocentric.Apis.Public.V0.Enums;
using Eurocentric.Infrastructure.Messaging;
using Microsoft.AspNetCore.Routing;
using SlimMessageBus;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;
using MethodRule = ArchUnitNET.Fluent.Syntax.Elements.Members.MethodMembers.MethodMembersShouldConjunction;

namespace Eurocentric.Apis.Public.ArchitectureTests;

[Category("architecture")]
public sealed class PublicApiArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(VotingMethod).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> DtoClasses = Classes()
        .That().ResideInNamespaceMatching(".Dtos")
        .And().AreNotAbstract();

    private static readonly IObjectProvider<Class> FeatureClasses = Classes()
        .That().ResideInNamespaceMatching(".Features.")
        .And().AreNotNested();

    private static readonly IObjectProvider<Class> FeatureQueryClasses = Classes()
        .That().AreNestedIn(FeatureClasses)
        .And().HaveName("Query");

    private static readonly IObjectProvider<Class> FeatureQueryHandlerClasses = Classes()
        .That().AreNestedIn(FeatureClasses)
        .And().HaveName("QueryHandler");

    private static readonly IObjectProvider<Class> FeatureRequestClasses = Classes()
        .That().AreNestedIn(FeatureClasses)
        .And().HaveName("Request");

    private static readonly IObjectProvider<Class> FeatureResponseClasses = Classes()
        .That().AreNestedIn(FeatureClasses)
        .And().HaveName("Response");

    private static readonly IObjectProvider<IType> DomainAssemblyTypes = Types()
        .That().ResideInNamespaceMatching("Eurocentric.Domain");

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Dto_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DtoClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Dto_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DtoClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Dto_classes_should_not_depend_on_any_domain_assembly_types()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(DtoClasses)
            .Should().NotDependOnAny(DomainAssemblyTypes);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_static()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_have_at_most_1_non_private_method_and_it_should_be_used_for_endpoint_mapping()
    {
        // Arrange
        MethodRule rule = MethodMembers()
            .That().AreDeclaredIn(FeatureClasses)
            .And().AreNotPrivate()
            .Should().HaveNameStartingWith("Map")
            .AndShould().FollowCustomCondition(new MapEndpointMethodName())
            .AndShould().FollowCustomCondition(new HasSingleIEndpointRouteBuilderParameter())
            .AndShould().HaveReturnType(typeof(IEndpointRouteBuilder));

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequest_should_implement_IQuery()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ImplementInterface(typeof(IRequest<>))
            .Should().ImplementInterface(typeof(IQuery<>));

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequest_should_be_nested_in_feature_classes()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ImplementInterface(typeof(IRequest<>))
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Classes_that_implement_IQuery_should_be_feature_query_or_request_classes()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().ImplementInterface(typeof(IQuery<>))
            .Should().Be(FeatureQueryClasses)
            .OrShould().Be(FeatureRequestClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_request_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureRequestClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_request_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureRequestClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_request_classes_should_not_depend_on_any_domain_assembly_types()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureRequestClasses)
            .Should().NotDependOnAny(DomainAssemblyTypes);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_query_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureQueryClasses)
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_query_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureQueryClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_query_classes_should_not_depend_on_any_domain_assembly_types()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureQueryClasses)
            .Should().NotDependOnAny(DomainAssemblyTypes);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_response_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureResponseClasses)
            .Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_response_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureResponseClasses)
            .Should().BeImmutable()
            .AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_response_classes_should_not_depend_on_any_domain_assembly_types()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().Are(FeatureResponseClasses)
            .Should().NotDependOnAny(DomainAssemblyTypes);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequestHandler_should_implement_IQueryHandler()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().ImplementInterface(typeof(IQueryHandler<,>));

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_that_implement_IRequestHandler_should_be_nested_in_feature_classes()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Classes_that_implement_IQueryHandler_should_be_feature_query_handler_classes()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().ImplementInterface(typeof(IQueryHandler<,>))
            .Should().Be(FeatureQueryHandlerClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;

    private sealed class MapEndpointMethodName : ICondition<MethodMember>
    {
        public string Description => "method name should be 'Map' + declaring type name";

        public IEnumerable<ConditionResult> Check(IEnumerable<MethodMember> objects, Architecture architecture) =>
            from methodMember in objects
            let expectedName = "Map" + methodMember.DeclaringType.Name
            let actualName = methodMember.Name.Split('(').First()
            let passed = actualName.Equals(expectedName, StringComparison.InvariantCulture)
            let failureMessage = passed ? null : CreateFailureMessage(expectedName, actualName)
            select new ConditionResult(methodMember, passed, failureMessage);

        public bool CheckEmpty() => true;

        private static string CreateFailureMessage(string expectedName, string? actualName) =>
            $"method name should be '{expectedName}', but it was '{actualName}'";
    }

    private sealed class HasSingleIEndpointRouteBuilderParameter : ICondition<MethodMember>
    {
        private const string ExpectedTypeFullName = "Microsoft.AspNetCore.Routing.IEndpointRouteBuilder";

        public string Description => "method should have a single parameter, which is of type 'IEndpointRouteBuilder'";

        public IEnumerable<ConditionResult> Check(IEnumerable<MethodMember> objects, Architecture architecture) =>
            from methodMember in objects
            let singleParameter = methodMember.Parameters.Single()
            let passed = singleParameter.FullName.Equals(ExpectedTypeFullName, StringComparison.InvariantCulture)
            select new ConditionResult(methodMember, passed);

        public bool CheckEmpty() => true;
    }
}
