using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed class AllAssembliesTests : ArchitectureTest
{
    private static readonly IObjectProvider<IType> AllTypes = Types()
        .That()
        .Are(AdminApiAssemblyTypes)
        .Or()
        .Are(ComponentsAssemblyTypes)
        .Or()
        .Are(DomainAssemblyTypes)
        .Or()
        .Are(PublicApiAssemblyTypes)
        .Or()
        .Are(WebAppAssemblyTypes);

    [Test]
    public async Task Public_types_should_not_be_nested()
    {
        // Arrange
        TypeRule rule = Types().That().Are(AllTypes).And().ArePublic().Should().NotBeNested();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(AllTypes).And().AreNotAbstract().Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
