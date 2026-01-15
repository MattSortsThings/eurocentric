using Eurocentric.Apis.Admin;
using Eurocentric.Apis.Public;
using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Placeholders;
using Assembly = System.Reflection.Assembly;

namespace Eurocentric.ArchitectureTests.TestUtils;

/// <summary>
///     Abstract base class for all architecture tests.
/// </summary>
[UseQualifiedDisplayName]
[Category("architecture")]
[ParallelLimiter<ParallelLimit>]
public abstract class ArchitectureTestBase
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
        .HaveName("Program")
        .Or()
        .ResideInNamespace("Eurocentric.Components.DataAccess.EfCore.Migrations");

    private protected static readonly IObjectProvider<IType> TypesInWebAppAssembly = Types()
        .That()
        .ResideInAssembly(typeof(Program).Assembly)
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> TypesInComponentsAssembly = Types()
        .That()
        .ResideInAssembly(typeof(IEndpointMapper).Assembly)
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> TypesInDomainAssembly = Types()
        .That()
        .ResideInAssembly(typeof(BlobbyGenerator).Assembly)
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> TypesInAdminApiAssembly = Types()
        .That()
        .ResideInAssembly(typeof(AdminApiEndpoints).Assembly)
        .And()
        .AreNot(IgnoredTypes);

    private protected static readonly IObjectProvider<IType> TypesInPublicApiAssemblyTypes = Types()
        .That()
        .ResideInAssembly(typeof(PublicApiEndpoints).Assembly)
        .And()
        .AreNot(IgnoredTypes);
}
