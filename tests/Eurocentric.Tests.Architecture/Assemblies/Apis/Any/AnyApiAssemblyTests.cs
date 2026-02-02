using Eurocentric.Tests.Architecture.Utils;

namespace Eurocentric.Tests.Architecture.Assemblies.Apis.Any;

public abstract partial class AnyApiAssemblyTests : ArchitectureTests
{
    private static readonly IObjectProvider<IType> TypesInAnyApiAssembly = Types()
        .That()
        .Are(TypesInAdminApiAssembly)
        .Or()
        .Are(TypesInPublicApiAssemblyTypes)
        .As("in any API assembly");

    private static readonly IObjectProvider<IType> NonNestedFeatureTypesInAnyApiAssembly = Types()
        .That()
        .Are(TypesInAnyApiAssembly)
        .And()
        .ResideInNamespaceMatching(".V0.")
        .Or()
        .ResideInNamespaceMatching(".V1.")
        .And()
        .DoNotResideInNamespaceMatching(".Common")
        .And()
        .AreNotNested()
        .As("non-nested feature types in any API assembly");

    private static readonly IObjectProvider<Class> FeatureClassesInAnyApiAssembly = Classes()
        .That()
        .Are(NonNestedFeatureTypesInAnyApiAssembly)
        .And()
        .AreAbstract();

    private static readonly IObjectProvider<Class> FeatureContractClassesInAnyApiAssembly = Classes()
        .That()
        .Are(NonNestedFeatureTypesInAnyApiAssembly)
        .And()
        .AreNot(FeatureClassesInAnyApiAssembly);
}
