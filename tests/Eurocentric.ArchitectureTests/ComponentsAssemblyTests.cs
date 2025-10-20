using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class ComponentsAssemblyTests : ArchitectureTest
{
    [Test]
    public async Task Domain_service_implementations_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .ImplementAnyInterfaces(Interfaces().That().Are(DomainAssemblyTypes))
            .And()
            .Are(ComponentsAssemblyTypes)
            .And()
            .AreNot(AlwaysIgnoredTypes)
            .Should()
            .BeInternal();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
