namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    [Test]
    public async Task CommandHandler_classes_should_have_name_CommandHandler()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommandHandler).Should().HaveName("CommandHandler");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommandHandler).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_not_be_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommandHandler).Should().NotBeRecord();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task CommandHandler_classes_should_be_nested_in_Feature_classes()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommandHandler).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
