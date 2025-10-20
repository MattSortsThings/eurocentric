using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class DomainAssemblyTests : ArchitectureTest
{
    [Test]
    public async Task Enums_should_reside_in_Enums_namespace()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .AreEnums()
            .And()
            .Are(DomainAssemblyTypes)
            .And()
            .AreNot(AlwaysIgnoredTypes)
            .Should()
            .ResideInNamespaceMatching(".Enums$");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
