using Eurocentric.Components.EndpointMapping;
using Eurocentric.Domain.Core;
using SlimMessageBus;

namespace Eurocentric.ArchitectureTests;

public sealed partial class ApiAssembliesTests
{
    private static readonly IObjectProvider<IType> ApiAssembliesTypes = Types()
        .That()
        .Are(AdminApiAssemblyTypes)
        .Or()
        .Are(PublicApiAssemblyTypes);

    private static readonly IObjectProvider<IType> TypesThatImplementIEndpointMapper = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IEndpointMapper));

    private static readonly IObjectProvider<IType> TypesThatImplementIRequest = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IRequest<>));

    private static readonly IObjectProvider<IType> TypesThatImplementIRequestHandler = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IRequestHandler<,>));

    private static readonly IObjectProvider<Class> FeatureClasses = Classes()
        .That()
        .ResideInNamespaceMatching(".Features.")
        .And()
        .DoNotHaveNameEndingWith("Request")
        .And()
        .DoNotHaveNameEndingWith("Response")
        .And()
        .AreNotNested();

    private static readonly IObjectProvider<IType> TypesThatImplementICommand = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(ICommand<>));

    private static readonly IObjectProvider<IType> TypesThatImplementICommandHandler = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(ICommandHandler<,>));

    private static readonly IObjectProvider<IType> TypesThatImplementIQuery = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IQuery<>));

    private static readonly IObjectProvider<IType> TypesThatImplementIQueryHandler = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IQueryHandler<,>));

    private static readonly IObjectProvider<IType> TypesThatImplementIUnitCommand = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IUnitCommand));

    private static readonly IObjectProvider<IType> TypesThatImplementIUnitCommandHandler = Types()
        .That()
        .Are(ApiAssembliesTypes)
        .And()
        .AreNot(AlwaysIgnoredTypes)
        .And()
        .ImplementInterface(typeof(IUnitCommandHandler<>));
}
