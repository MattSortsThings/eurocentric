using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

public abstract class ApiModule : IApiModule
{
    public abstract string ApiName { get; }

    public abstract string Prefix { get; }

    public void MapVersionedApiEndpoints(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiName).MapGroup(Prefix);

        foreach (ApiRelease apiRelease in GetAllEndpointsInAssembly().MapToApiReleases(ApiName))
        {
            apiGroup.MapApiRelease(apiRelease);
        }
    }

    private IApiEndpoint[] GetAllEndpointsInAssembly() =>
        GetType().Assembly.GetTypes()
            .Where(type => typeof(IApiEndpoint).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
            .Select(type => (IApiEndpoint)Activator.CreateInstance(type)!)
            .ToArray();
}
