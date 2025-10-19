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

    private protected static readonly IObjectProvider<IType> IgnoredTypes = Types()
        .That()
        .ResideInNamespaceMatching(".EfCore.Migrations")
        .Or()
        .HaveName("Program")
        .As("Ignored Types");

    private protected static bool Passed(EvaluationResult result) => result.Passed;
}
