using Eurocentric.Domain.Abstractions.Errors;
using Assembly = System.Reflection.Assembly;

namespace Eurocentric.Tests.Architecture.Utils;

[Category("architecture")]
public abstract class ArchitectureTests
{
    private protected static readonly ArchUnitNET.Domain.Architecture ArchitectureUnderTest = new ArchLoader()
        .LoadAssemblies(
            Assembly.Load("Eurocentric.Apis.Admin"),
            Assembly.Load("Eurocentric.Apis.Public"),
            Assembly.Load("Eurocentric.Components"),
            Assembly.Load("Eurocentric.Domain")
        )
        .Build();

    private protected static readonly IObjectProvider<IType> IgnoredTypes = Types()
        .That()
        .HaveName("Program")
        .Or()
        .ResideInNamespace("Eurocentric.Components.DataAccess.EfCore.Migrations");

    private protected static readonly IObjectProvider<IType> DomainAssemblyTypes = Types()
        .That()
        .ResideInAssembly(typeof(IDomainError).Assembly)
        .And()
        .AreNot(IgnoredTypes);
}
