using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.ApiModules;

internal static class RouteGroupBuilderExtensions
{
    internal static void MapApiRelease(this RouteGroupBuilder builder, ApiRelease apiRelease)
    {
        var (apiVersion, groupName, endpoints) = apiRelease;

        RouteGroupBuilder apiReleaseGroup = builder.MapGroup("v{version:apiVersion}")
            .HasApiVersion(apiVersion)
            .WithGroupName(groupName);

        foreach (IApiEndpoint endpoint in endpoints)
        {
            endpoint.Map(apiReleaseGroup).WithName(groupName + "." + endpoint.DisplayName);
        }
    }
}
