using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed partial class DomainAssemblyTests : ArchitectureTest
{
    private static readonly IObjectProvider<IType> EnumTypes = Types().That().Are(DomainAssemblyTypes).And().AreEnums();

    [Test]
    public async Task Enum_types_should_be_public()
    {
        // Arrange
        TypeRule rule = Types().That().Are(EnumTypes).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Enum_types_should_reside_in_Enums_namespace()
    {
        // Arrange
        TypeRule rule = Types().That().Are(EnumTypes).Should().ResideInNamespace("Eurocentric.Domain.Enums");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
