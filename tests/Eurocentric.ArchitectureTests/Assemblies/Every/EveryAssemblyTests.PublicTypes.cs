using ArchUnitNET.TUnit;

namespace Eurocentric.ArchitectureTests.Assemblies.Every;

public abstract partial class EveryAssemblyTests
{
    public sealed class PublicTypes : EveryAssemblyTests
    {
        [Test]
        public void Should_not_be_nested()
        {
            Types()
                .That()
                .Are(TypesInEveryAssembly)
                .And()
                .ArePublic()
                .Should()
                .NotBeNested()
                .Check(ArchitectureUnderTest);
        }
    }
}
