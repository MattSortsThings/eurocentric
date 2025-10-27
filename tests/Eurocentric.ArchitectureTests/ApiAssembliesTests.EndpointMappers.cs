using Eurocentric.Components.EndpointMapping;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    private static readonly IObjectProvider<Class> EndpointMapperClasses = Classes()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ImplementInterface(typeof(IEndpointMapper));

    [Test]
    public async Task EndpointMapper_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EndpointMapperClasses).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task EndpointMapper_classes_should_be_nested_in_Feature_classes()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EndpointMapperClasses).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task EndpointMapper_classes_should_have_name_EndpointMapper()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EndpointMapperClasses).Should().HaveName("EndpointMapper");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task EndpointMapper_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EndpointMapperClasses).Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
