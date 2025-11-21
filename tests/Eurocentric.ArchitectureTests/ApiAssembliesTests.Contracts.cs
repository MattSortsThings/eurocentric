using Eurocentric.Components.OpenApi;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    private static readonly IObjectProvider<Class> DtoClasses = Classes()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.*.*.Dtos.")
        .And()
        .AreNotAbstract();

    private static readonly IObjectProvider<Class> FeatureRequestClasses = Classes()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.*.*.Features.*.")
        .And()
        .HaveNameEndingWith("Request");

    private static readonly IObjectProvider<Class> FeatureResponseClasses = Classes()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.*.*.Features.*.")
        .And()
        .HaveNameEndingWith("Response");

    [Test]
    public async Task DTO_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DtoClasses).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DTO_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DtoClasses).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DTO_classes_should_implement_IDtoSchemaExampleProvider()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(DtoClasses)
            .Should()
            .ImplementInterface(typeof(IDtoSchemaExampleProvider<>));

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_request_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureRequestClasses).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_request_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureRequestClasses).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_response_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureResponseClasses).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_response_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureResponseClasses).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
