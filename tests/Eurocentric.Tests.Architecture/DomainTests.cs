using Eurocentric.Tests.Architecture.Utils;

namespace Eurocentric.Tests.Architecture;

public sealed class DomainTests : ArchitectureTests
{
    [Test]
    public async Task Classes_in_Abstractions_namespace_should_be_abstract()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(DomainAssemblyTypes)
            .And()
            .ResideInNamespaceMatching("Eurocentric.Domain.Abstractions")
            .Should()
            .BeAbstract();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).AllPassed();
    }
}
