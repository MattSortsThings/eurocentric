using Assembly = System.Reflection.Assembly;

namespace Eurocentric.ArchitectureTests.TestUtils;

[Category("architecture")]
public abstract class ArchitectureTest
{
    private protected static readonly Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssemblies(
            Assembly.Load("Eurocentric.Apis.Admin"),
            Assembly.Load("Eurocentric.Apis.Public"),
            Assembly.Load("Eurocentric.Components"),
            Assembly.Load("Eurocentric.Domain"),
            Assembly.Load("Eurocentric.WebApp")
        )
        .Build();

    private static readonly IObjectProvider<IType> IgnoredTypes = Types()
        .That()
        .ResideInNamespaceMatching(".EfCore.Migrations")
        .Or()
        .HaveName("Program")
        .As("Ignored Types");

    private protected static readonly IObjectProvider<IType> WebAppAssemblyTypes = Types()
        .That()
        .ResideInAssemblyMatching("Eurocentric.WebApp")
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> AdminApiAssemblyTypes = Types()
        .That()
        .ResideInAssemblyMatching("Eurocentric.Apis.Admin")
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> PublicApiAssemblyTypes = Types()
        .That()
        .ResideInAssemblyMatching("Eurocentric.Apis.Public")
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> ComponentsAssemblyTypes = Types()
        .That()
        .ResideInAssemblyMatching("Eurocentric.Components")
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> DomainAssemblyTypes = Types()
        .That()
        .ResideInAssemblyMatching("Eurocentric.Domain")
        .And()
        .AreNot(IgnoredTypes);

    private protected static bool Passed(EvaluationResult result) => result.Passed;
}
