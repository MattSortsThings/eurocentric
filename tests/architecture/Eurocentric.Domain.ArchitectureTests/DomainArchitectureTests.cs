using ArchUnitNET.Domain;
using ArchUnitNET.Fluent.Syntax.Elements.Types;
using ArchUnitNET.Fluent.Syntax.Elements.Types.Classes;
using ArchUnitNET.Loader;
using Eurocentric.Domain.Abstractions;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Domain.ArchitectureTests;

[Category("architecture")]
public sealed class DomainArchitectureTests
{
    private static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssembly(typeof(ValueObject).Assembly)
        .Build();

    private static readonly IObjectProvider<Class> DomainEntityClasses = Classes()
        .That().ResideInNamespaceMatching(@"\.Aggregates\.")
        .And().ArePublic()
        .And().AreNotAbstract()
        .And().DoNotHaveNameEndingWith("Builder");

    [Test]
    public async Task Public_non_abstract_classes_should_be_sealed()
    {
        // Arrange
        ClassesShouldConjunction rule = Classes()
            .That().ArePublic()
            .And().AreNotAbstract()
            .Should().BeSealed();

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest)).ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Public_enums_should_reside_in_namespace_ending_Enums()
    {
        // Arrange
        TypesShouldConjunction rule = Types()
            .That().ArePublic()
            .And().AreEnums()
            .Should().ResideInNamespaceMatching(@"\.Enums$");

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest))
            .ContainsOnly(result => result.Passed);
    }

    [Test]
    public async Task Domain_entity_classes_should_inherit_Entity_base_class()
    {
        ClassesShouldConjunction rule = Classes()
            .That().Are(DomainEntityClasses)
            .Should().BeAssignableTo(typeof(Entity));

        // Assert
        await Assert.That(rule.Evaluate(ArchitectureUnderTest))
            .ContainsOnly(result => result.Passed);
    }
}
