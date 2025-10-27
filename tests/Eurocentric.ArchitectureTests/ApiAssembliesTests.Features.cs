using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    private static readonly IObjectProvider<Class> FeatureClasses = Classes()
        .That()
        .Are(Types().That().Are(AdminApiAssemblyTypes).Or().Are(PublicApiAssemblyTypes))
        .And()
        .ResideInNamespaceMatching("Eurocentric.Apis.*.*.Features.*.")
        .And()
        .AreNotNested()
        .And()
        .DoNotHaveNameEndingWith("Request")
        .And()
        .DoNotHaveNameEndingWith("Response");

    [Test]
    public async Task Feature_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureClasses).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_have_no_public_members()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(FeatureClasses)
            .Should()
            .FollowCustomCondition(new ClassHasNoPublicMembersCondition());

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
