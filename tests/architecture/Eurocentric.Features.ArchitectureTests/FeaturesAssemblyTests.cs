using ArchUnitNET.Domain;
using ArchUnitNET.Domain.Extensions;
using ArchUnitNET.Fluent.Predicates;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnitV3;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Eurocentric.Features.ArchitectureTests;

[Trait("Category", "Architecture")]
public sealed class FeaturesAssemblyTests
{
    private static readonly Architecture Architecture = new ArchLoader()
        .LoadAssembly(typeof(Middleware).Assembly)
        .Build();

    [Fact]
    [Trait("Category", "Placeholder")]
    public void Should_always_pass() => Classes()
        .That().FollowCustomPredicate(new ModelsPredicate())
        .Should().BeSealed()
        .Check(Architecture);

    private class ModelsPredicate : IPredicate<Class>
    {
        public IEnumerable<Class> GetMatchingObjects(IEnumerable<Class> objects, Architecture architecture) =>
            objects.Where(@class => @class.Namespace.NameEndsWith(".Models"));

        public string Description => "namespace ends with '.Models'";
    }
}
