using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public sealed class AnyAssemblyTests_NonAbstractClasses : AnyAssemblyTests
{
    [Test]
    public void Should_be_sealed()
    {
        Classes()
            .That()
            .Are(TypesInAnyAssembly)
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .Check(ArchitectureUnderTest);
    }
}
