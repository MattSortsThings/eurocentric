using Eurocentric.Domain.Core;

namespace Eurocentric.ArchitectureTests;

public sealed partial class DomainAssemblyTests
{
    private static readonly IObjectProvider<IType> DomainEventTypes = Types()
        .That()
        .Are(DomainAssemblyTypes)
        .And()
        .ImplementInterface(typeof(IDomainEvent))
        .And()
        .DoNotResideInNamespace("Eurocentric.Domain.Core");

    [Test]
    public async Task DomainEvent_classes_should_reside_in_Events_namespace()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventTypes).Should().ResideInNamespace("Eurocentric.Domain.Events");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventTypes).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_be_immutable_records()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventTypes).Should().BeImmutable().AndShould().BeRecord();

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }

    [Test]
    public async Task DomainEvent_classes_should_have_name_ending_with_Event()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(DomainEventTypes).Should().HaveNameEndingWith("Event");

        // Act
        IEnumerable<EvaluationResult> result = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(result).ContainsOnly(Passed);
    }
}
