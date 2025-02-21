using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

/// <summary>
///     Abstract base class for an API module.
/// </summary>
/// <remarks>Define a concrete derivative of this abstract base class in each API project.</remarks>
public abstract class ApiModule : IApiEndpointsMapper
{
    /// <summary>
    ///     Gets the unique name of the API.
    /// </summary>
    /// <example>
    ///     <c>"CustomersApi"</c>
    /// </example>
    protected abstract string ApiName { get; }

    /// <summary>
    ///     Gets the unique URL prefix for the API, which must include a version segment.
    /// </summary>
    /// <example>
    ///     <c>"customers/api/v{version:ApiVersion}"</c>
    /// </example>
    protected abstract string UrlPrefix { get; }

    /// <summary>
    ///     Gets the unique endpoint group name for the API, which is used to generate documentation URLs.
    /// </summary>
    /// <example>
    ///     <c>"customers-api"</c>
    /// </example>
    protected abstract string EndpointGroupName { get; }

    public void Map(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiName)
            .MapGroup(UrlPrefix)
            .WithGroupName(EndpointGroupName);

        foreach (Action<IEndpointRouteBuilder> mapper in GetEndpointMappers())
        {
            mapper(apiGroup);
        }
    }

    private IEnumerable<Action<IEndpointRouteBuilder>> GetEndpointMappers()
    {
        (ApiVersion[] apiVersions, IApiEndpoint[] endpoints) = ScanAssemblyForVersionedApiEndpointTypes();

        foreach (IApiEndpoint endpoint in endpoints)
        {
            yield return builder => endpoint.Map(builder)
                .WithName(endpoint.EndpointName)
                .WithApiVersions(apiVersions.Where(version => endpoint.InitialApiVersion.IsIncludedIn(version)));
        }
    }


    private (ApiVersion[], IApiEndpoint[]) ScanAssemblyForVersionedApiEndpointTypes()
    {
        IApiEndpoint[] endpoints = GetType().Assembly.GetTypes()
            .Where(type => typeof(IApiEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => (IApiEndpoint)Activator.CreateInstance(type)!)
            .ToArray();

        ApiVersion[] apiVersions = endpoints.Select(endpoint => endpoint.InitialApiVersion)
            .Distinct()
            .OrderBy(version => version)
            .ToArray();

        return (apiVersions, endpoints);
    }
}
