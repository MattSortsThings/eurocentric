using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class AdminApiAssemblyTests : ArchitectureTest
{
    [Test]
    public async Task Types_in_V0_namespace_should_not_depend_on_any_types_in_V1_namespace()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .Are(AdminApiAssemblyTypes)
            .And()
            .ResideInNamespaceMatching("^Eurocentric.Apis.Admin.V0")
            .And()
            .AreNot(AlwaysIgnoredTypes)
            .Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching("^Eurocentric.Apis.Admin.V1");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_in_V1_namespace_should_not_depend_on_any_types_in_V0_namespace()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .Are(AdminApiAssemblyTypes)
            .And()
            .ResideInNamespaceMatching("^Eurocentric.Apis.Admin.V1")
            .And()
            .AreNot(AlwaysIgnoredTypes)
            .Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching("^Eurocentric.Apis.Admin.V0");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
