using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public abstract partial class AnyAssemblyTests
{
    public sealed class PublicTypes : AnyAssemblyTests
    {
        private static readonly IObjectProvider<IType> PublicTypesInAnyAssembly = Types()
            .That()
            .Are(TypesInAnyAssembly)
            .And()
            .ArePublic()
            .As("public types in any assembly");

        [Test]
        public void Should_not_be_nested() =>
            Types().That().Are(PublicTypesInAnyAssembly).Should().NotBeNested().Check(ArchitectureUnderTest);
    }
}
