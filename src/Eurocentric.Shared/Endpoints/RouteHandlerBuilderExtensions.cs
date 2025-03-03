using Asp.Versioning;
using Microsoft.AspNetCore.Builder;

namespace Eurocentric.Shared.Endpoints;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder HasApiVersions(this RouteHandlerBuilder builder, IEnumerable<ApiVersion> apiVersions)
    {
        foreach (ApiVersion apiVersion in apiVersions)
        {
            builder.HasApiVersion(apiVersion);
        }

        return builder;
    }
}
