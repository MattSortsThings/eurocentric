using Eurocentric.ArchitectureTests.TestUtils;
using Eurocentric.Domain.Core;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests : ArchitectureTest
{
    [Test]
    public async Task Request_types_should_implement_ICommand_IQuery_or_IUnitCommand()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .Are(TypesThatImplementIRequest)
            .Should()
            .ImplementInterface(typeof(ICommand<>))
            .OrShould()
            .ImplementInterface(typeof(IQuery<>))
            .OrShould()
            .ImplementInterface(typeof(IUnitCommand));

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task RequestHandler_types_should_implement_ICommandHandler_IQueryHandler_or_IUnitCommandHandler()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .Are(TypesThatImplementIRequestHandler)
            .Should()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .OrShould()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .OrShould()
            .ImplementInterface(typeof(IUnitCommandHandler<>));

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
