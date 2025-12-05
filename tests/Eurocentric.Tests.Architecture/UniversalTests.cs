using Eurocentric.Tests.Architecture.Utils;

namespace Eurocentric.Tests.Architecture;

public sealed class UniversalTests : ArchitectureTests
{
    [Test]
    public async Task Public_classes_should_be_sealed()
    {
        // Arrange
        ClassRule rule = Classes().That().ArePublic().And().AreNot(IgnoredTypes).Should().BeSealed();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).AllPassed();
    }

    [Test]
    public async Task Public_types_should_not_be_nested()
    {
        // Arrange
        TypeRule rule = Types().That().ArePublic().And().AreNot(IgnoredTypes).Should().NotBeNested();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).AllPassed();
    }

    [Test]
    public async Task Enum_types_should_reside_in_namespace_ending_with_Enums()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .AreEnums()
            .And()
            .AreNot(IgnoredTypes)
            .Should()
            .ResideInNamespaceMatching(".Enums$");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).AllPassed();
    }
}
