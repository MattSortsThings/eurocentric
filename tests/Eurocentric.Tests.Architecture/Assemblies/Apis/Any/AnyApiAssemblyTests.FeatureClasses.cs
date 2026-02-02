using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Apis.Any;

public abstract partial class AnyApiAssemblyTests
{
    public sealed class FeatureClasses : AnyApiAssemblyTests
    {
        [Test]
        public void Should_be_internal() =>
            Classes().That().Are(FeatureClassesInAnyApiAssembly).Should().BeInternal().Check(ArchitectureUnderTest);
    }
}
