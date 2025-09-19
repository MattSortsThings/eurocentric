using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using Eurocentric.Domain.V0Analytics.Rankings.CompetingCountries;
using Eurocentric.Domain.V0Analytics.Scoreboard;
using Eurocentric.Infrastructure.Messaging;
using static ArchUnitNET.Fluent.ArchRuleDefinition;
using ClassRule = ArchUnitNET.Fluent.Syntax.Elements.Types.Classes.ClassesShouldConjunction;
using TypeRule = ArchUnitNET.Fluent.Syntax.Elements.Types.TypesShouldConjunction;

namespace Eurocentric.Infrastructure.ArchitectureTests;

[Category("architecture")]
public sealed class InfrastructureArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(IQuery<>).Assembly)
        .Build();

    private static readonly IObjectProvider<IType> ExcludedTypes = Types()
        .That().ResideInNamespaceMatching("Eurocentric.Infrastructure.DataAccess.EfCore.Migrations");

    [Test]
    public async Task Public_types_should_not_be_nested()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ArePublic()
            .And().AreNot(ExcludedTypes)
            .Should().NotBeNested();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes()
            .That().AreNotAbstract()
            .And().AreNot(ExcludedTypes)
            .Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Domain_gateway_implementations_should_be_internal()
    {
        // Arrange
        TypeRule rule = Types()
            .That().ImplementInterface(typeof(IScoreboardGateway))
            .Or().ImplementInterface(typeof(ICompetingCountryRankingsGateway))
            .Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    private static bool Passed(EvaluationResult result) => result.Passed;
}
