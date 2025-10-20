namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    [Test]
    public async Task Command_classes_should_have_name_Command()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommand).Should().HaveName("Command");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_internal()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommand).Should().BeInternal();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommand).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Command_classes_should_be_nested_in_Feature_classes()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(TypesThatImplementICommand).Should().BeNestedIn(FeatureClasses);

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }
}
