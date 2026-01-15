using ArchUnitNET.TUnit;

namespace Eurocentric.ArchitectureTests.Assemblies.Every;

public abstract partial class EveryAssemblyTests
{
    public sealed class NonAbstractClasses : EveryAssemblyTests
    {
        [Test]
        public void Should_be_sealed()
        {
            Classes()
                .That()
                .Are(TypesInEveryAssembly)
                .And()
                .AreNotAbstract()
                .Should()
                .BeSealed()
                .Check(ArchitectureUnderTest);
        }
    }
}
