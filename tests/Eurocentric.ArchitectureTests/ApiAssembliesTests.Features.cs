namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    [Test]
    public async Task Feature_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureClasses).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_be_static()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(FeatureClasses).Should().BeSealed().AndShould().BeAbstract();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Feature_classes_should_have_no_public_members()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(FeatureClasses)
            .Should()
            .FollowCustomCondition(
                cls => cls.Members.All(member => member.Visibility != Visibility.Public),
                "has no public members",
                "at least one public member was found"
            );

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
