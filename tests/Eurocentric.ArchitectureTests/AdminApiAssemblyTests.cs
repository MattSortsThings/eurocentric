using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class AdminApiAssemblyTests : ArchitectureTest
{
    private static readonly IObjectProvider<IType> V0Types = Types()
        .That()
        .Are(AdminApiAssemblyTypes)
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.Admin.V0");

    private static readonly IObjectProvider<IType> V1Types = Types()
        .That()
        .Are(AdminApiAssemblyTypes)
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.Admin.V1");

    [Test]
    public async Task Types_in_V0_namespace_should_not_depend_on_any_types_in_V1_namespace()
    {
        // Arrange
        TypeRule rule = Types().That().Are(V0Types).Should().NotDependOnAny(V1Types);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_in_V1_namespace_should_not_depend_on_any_types_in_V0_namespace()
    {
        // Arrange
        TypeRule rule = Types().That().Are(V1Types).Should().NotDependOnAny(V0Types);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
