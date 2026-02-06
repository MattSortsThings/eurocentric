using Eurocentric.Tests.Architecture.Utils;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public abstract class AnyAssemblyTests : ArchitectureTests
{
    private protected static readonly IObjectProvider<IType> TypesInAnyAssembly = Types()
        .That()
        .Are(TypesInAdminApiAssembly)
        .Or()
        .Are(TypesInPublicApiAssembly)
        .Or()
        .Are(TypesInComponentsAssembly)
        .Or()
        .Are(TypesInDomainAssembly)
        .Or()
        .Are(TypesInWebAppAssembly)
        .As("types in any assembly");
}
