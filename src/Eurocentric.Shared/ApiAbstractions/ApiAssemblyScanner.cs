using Asp.Versioning;

namespace Eurocentric.Shared.ApiAbstractions;

public abstract class ApiAssemblyScanner<TApiInfo> where TApiInfo : class, IApiInfo, new()
{
    private protected readonly TApiInfo ApiInfo = new();

    private protected (IEndpointInfo[] Endpoints, ApiVersion[] ApiVersions) ScanAssemblyForEndpointsAndApiVersions()
    {
        IEndpointInfo[] endpoints = ApiInfo.GetType().Assembly.GetTypes()
            .Where(type => typeof(IEndpointInfo).IsAssignableFrom(type) && type is { IsAbstract : false, IsInterface: false })
            .Select(type => (IEndpointInfo)Activator.CreateInstance(type)!)
            .ToArray();

        ApiVersion[] apiVersions = endpoints.GroupBy(endpoint => endpoint.MajorApiVersion)
            .SelectMany(grouping => grouping.GroupBy(endpoint => endpoint.MinorApiVersion)
                .Select(subGrouping => new ApiVersion(grouping.Key, subGrouping.Key)))
            .OrderBy(apiVersion => apiVersion)
            .ToArray();

        return (endpoints, apiVersions);
    }
}
