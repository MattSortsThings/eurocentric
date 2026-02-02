using Eurocentric.Tests.Architecture.Utils;

namespace Eurocentric.Tests.Architecture.Assemblies.Any;

public abstract partial class AnyAssemblyTests : ArchitectureTests
{
    private protected static readonly IObjectProvider<IType> TypesInAnyAssembly = Types()
        .That()
        .Are(TypesInWebAppAssembly)
        .Or()
        .Are(TypesInAdminApiAssembly)
        .Or()
        .Are(TypesInPublicApiAssemblyTypes)
        .Or()
        .Are(TypesInComponentsAssembly)
        .Or()
        .Are(TypesInDomainAssembly)
        .As("in any assembly");
}
