using Asp.Versioning;

namespace Eurocentric.Shared.ApiModules;

internal static class EndpointArrayExtensions
{
    internal static IEnumerable<ApiRelease> MapToApiReleases(this IApiEndpoint[] apiEndpoints, string apiName) =>
        from apiVersion in apiEndpoints.GetDistinctApiVersions()
        let groupName = $"{apiName}-v{apiVersion.MajorVersion}.{apiVersion.MinorVersion}"
        let endpoints = apiEndpoints.Where(endpoint => MatchingVersions(endpoint, apiVersion)).ToArray()
        select new ApiRelease(apiVersion, groupName, endpoints);

    private static bool MatchingVersions(this IApiEndpoint endpoint, ApiVersion apiVersion) =>
        endpoint.MajorVersion == apiVersion.MajorVersion && endpoint.MinorVersion <= apiVersion.MinorVersion;

    private static IEnumerable<ApiVersion> GetDistinctApiVersions(this IEnumerable<IApiEndpoint> endpoints) =>
        endpoints.Select(endpoint =>
                new ApiVersion(endpoint.MajorVersion, endpoint.MinorVersion))
            .Distinct();
}
