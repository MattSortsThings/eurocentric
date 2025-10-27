using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class ComponentsAssemblyTests : ArchitectureTest
{
    [Test]
    public async Task Domain_interface_implementations_should_be_internal()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .Are(ComponentsAssemblyTypes)
            .And()
            .ImplementAnyInterfaces(Interfaces().That().Are(DomainAssemblyTypes))
            .Should()
            .BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
