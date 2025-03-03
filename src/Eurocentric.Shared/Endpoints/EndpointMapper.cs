using Asp.Versioning;
using Eurocentric.Shared.ApiAbstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.Endpoints;

public sealed class EndpointMapper<TApiInfo> : IEndpointMapper
    where TApiInfo : class, IApiInfo, new()
{
    private readonly TApiInfo _apiInfo = new();

    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder api = app.NewVersionedApi(_apiInfo.Id)
            .MapGroup(_apiInfo.UrlPrefix)
            .WithGroupName(_apiInfo.EndpointGroupName);

        foreach (Action<IEndpointRouteBuilder> mapper in GetEndpointMappingActions())
        {
            mapper.Invoke(api);
        }

        Console.WriteLine(api);
    }

    private IEnumerable<Action<IEndpointRouteBuilder>> GetEndpointMappingActions()
    {
        var (endpoints, versions) = ScanAssemblyForEndpointsAndApiVersions();

        foreach (IEndpointInfo endpoint in endpoints)
        {
            yield return builder => InitializeRouteHandlerBuilder(endpoint)
                .Invoke(builder)
                .WithName(endpoint.Name)
                .HasApiVersions(GetApplicableApiVersions(endpoint, versions));
        }
    }


    private (IEndpointInfo[] Endpoints, ApiVersion[] ApiVersions) ScanAssemblyForEndpointsAndApiVersions()
    {
        IEndpointInfo[] endpoints = _apiInfo.GetType().Assembly.GetTypes()
            .Where(CanInstantiateAsEndpoint)
            .Select(type => (IEndpointInfo)Activator.CreateInstance(type)!)
            .ToArray();

        ApiVersion[] apiVersions = endpoints.GroupBy(endpoint => endpoint.MajorApiVersion)
            .SelectMany(grouping => grouping.GroupBy(endpoint => endpoint.MinorApiVersion)
                .Select(subGrouping => new ApiVersion(grouping.Key, subGrouping.Key)))
            .OrderBy(apiVersion => apiVersion)
            .ToArray();

        return (endpoints, apiVersions);
    }

    private static bool CanInstantiateAsEndpoint(Type type) =>
        typeof(IEndpointInfo).IsAssignableFrom(type) && type is { IsAbstract : false, IsInterface: false };

    private static IEnumerable<ApiVersion> GetApplicableApiVersions(IEndpointInfo endpoint, ApiVersion[] apiVersions)
    {
        (int major, int minor) = (endpoint.MajorApiVersion, endpoint.MinorApiVersion);

        return apiVersions.Where(version => version.MajorVersion == major && version.MinorVersion >= minor);
    }

    private static Func<IEndpointRouteBuilder, RouteHandlerBuilder> InitializeRouteHandlerBuilder(IEndpointInfo endpoint)
    {
        var (resource, handler, method) = (endpoint.Route, endpoint.Handler, endpoint.Method);

        if (method == HttpMethod.Get)
        {
            return builder => builder.MapGet(resource, handler);
        }

        if (method == HttpMethod.Post)
        {
            return builder => builder.MapPost(resource, handler);
        }

        if (method == HttpMethod.Put)
        {
            return builder => builder.MapPut(resource, handler);
        }

        if (method == HttpMethod.Delete)
        {
            return builder => builder.MapDelete(resource, handler);
        }

        throw new InvalidOperationException($"Method {method} is not supported.");
    }
}
