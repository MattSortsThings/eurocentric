using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Conditions;
using Eurocentric.ArchitectureTests.TestUtils;
using Eurocentric.Domain.Core;

namespace Eurocentric.ArchitectureTests;

public sealed class DomainAssemblyTests : ArchitectureTest
{
    private static readonly IObjectProvider<Class> EntityClasses = Classes()
        .That()
        .AreAssignableTo(typeof(Entity))
        .And()
        .Are(DomainAssemblyTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .AreNot(typeof(Entity), typeof(AggregateRoot<>));

    private static readonly IObjectProvider<Class> ValueObjectClasses = Classes()
        .That()
        .AreAssignableTo(typeof(ValueObject))
        .And()
        .Are(DomainAssemblyTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .AreNot(
            typeof(ValueObject),
            typeof(GuidAtomicValueObject),
            typeof(Int32AtomicValueObject),
            typeof(StringAtomicValueObject),
            typeof(CompoundValueObject)
        );

    [Test]
    public async Task Enums_should_reside_in_Enums_namespace()
    {
        // Arrange
        TypeRule rule = Types()
            .That()
            .AreEnums()
            .And()
            .Are(DomainAssemblyTypes)
            .And()
            .AreNot(AlwaysIgnoredTypes)
            .Should()
            .ResideInNamespaceMatching(".Enums$");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_reside_in_Aggregates_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(EntityClasses)
            .Should()
            .ResideInNamespaceMatching("^Eurocentric.Domain.Aggregates.");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_be_public()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(EntityClasses).Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Entity_classes_should_have_a_private_or_private_protected_parameterless_constructor()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(EntityClasses)
            .Should()
            .FollowCustomCondition(new HasPrivateOrPrivateProtectedParameterlessConstructorCondition());

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_reside_in_ValueObjects_namespace()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(ValueObjectClasses)
            .Should()
            .ResideInNamespace("Eurocentric.Domain.ValueObjects");

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task ValueObject_classes_should_be_immutable()
    {
        // Arrange
        ClassRule rule = Classes().That().Are(ValueObjectClasses).Should().BeImmutable();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task AtomicValueObject_classes_should_have_single_constructor_which_is_private()
    {
        // Arrange
        ClassRule rule = Classes()
            .That()
            .Are(ValueObjectClasses)
            .And()
            .AreAssignableTo(
                typeof(GuidAtomicValueObject),
                typeof(Int32AtomicValueObject),
                typeof(StringAtomicValueObject)
            )
            .Should()
            .FollowCustomCondition(new HasSingleConstructorWhichIsPrivateCondition());

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    [Test]
    public async Task Types_in_ValueObjects_namespace_should_be_public()
    {
        // Arrange
        TypeRule rule = Types().That().ResideInNamespace("Eurocentric.Domain.ValueObjects").Should().BePublic();

        // Act
        IEnumerable<EvaluationResult> results = rule.Evaluate(ArchitectureUnderTest);

        // Assert
        await Assert.That(results).ContainsOnly(Passed);
    }

    private sealed class HasPrivateOrPrivateProtectedParameterlessConstructorCondition : ICondition<Class>
    {
        public string Description => "to have a private or private protected parameterless constructor";

        public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
        {
            foreach (Class @class in objects)
            {
                if (
                    @class
                        .GetConstructors()
                        .Any(member =>
                            member.Visibility is Visibility.Private or Visibility.PrivateProtected
                            && !member.Parameters.Any()
                        )
                )
                {
                    yield return new ConditionResult(@class, true);
                }
                else
                {
                    yield return new ConditionResult(
                        @class,
                        false,
                        "private or private protected parameterless constructor does not exist"
                    );
                }
            }
        }

        public bool CheckEmpty() => true;
    }

    private sealed class HasSingleConstructorWhichIsPrivateCondition : ICondition<Class>
    {
        public string Description => "to have a single constructor, which is private";

        public IEnumerable<ConditionResult> Check(IEnumerable<Class> objects, Architecture architecture)
        {
            foreach (Class @class in objects)
            {
                MethodMember[] constructors = @class.GetConstructors().ToArray();

                if (constructors.Length != 1)
                {
                    yield return new ConditionResult(@class, false, $"constructors count was {constructors.Length}");
                }
                else if (constructors.Single().Visibility is { } visibility && visibility != Visibility.Private)
                {
                    yield return new ConditionResult(@class, false, $"single constructor visibility was {visibility}");
                }
                else
                {
                    yield return new ConditionResult(@class, true);
                }
            }
        }

        public bool CheckEmpty() => true;
    }
}
