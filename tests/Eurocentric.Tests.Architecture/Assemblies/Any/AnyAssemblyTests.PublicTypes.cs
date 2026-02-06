using ArchUnitNET.TUnit;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public sealed class AnyAssemblyTests_PublicTypes : AnyAssemblyTests
{
    [Test]
    public void Should_not_be_nested() =>
        Types().That().Are(TypesInAnyAssembly).And().ArePublic().Should().NotBeNested().Check(ArchitectureUnderTest);
}
