using Eurocentric.ArchitectureTests.TestUtils;

namespace Eurocentric.ArchitectureTests.Assemblies.Every;

public abstract partial class EveryAssemblyTests : ArchitectureTestBase
{
    private static readonly IObjectProvider<IType> TypesInEveryAssembly = Types()
        .That()
        .Are(TypesInWebAppAssembly)
        .Or()
        .Are(TypesInAdminApiAssembly)
        .Or()
        .Are(TypesInPublicApiAssemblyTypes)
        .Or()
        .Are(TypesInComponentsAssembly)
        .Or()
        .Are(TypesInDomainAssembly);
}
