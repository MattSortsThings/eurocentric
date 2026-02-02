using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public abstract partial class AnyAssemblyTests
{
    public sealed class NonAbstractClasses : AnyAssemblyTests
    {
        private static readonly IObjectProvider<Class> NonAbstractClassesInAnyAssembly = Classes()
            .That()
            .Are(TypesInAnyAssembly)
            .And()
            .AreNotAbstract()
            .As("non-abstract classes in any assembly");

        [Test]
        public void Should_be_sealed() =>
            Classes().That().Are(NonAbstractClassesInAnyAssembly).Should().BeSealed().Check(ArchitectureUnderTest);
    }
}
