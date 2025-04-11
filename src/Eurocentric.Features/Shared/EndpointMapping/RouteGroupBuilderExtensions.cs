using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.Shared.EndpointMapping;

internal static class RouteGroupBuilderExtensions
{
    internal static RouteHandlerBuilder MapEndpoint(this RouteGroupBuilder builder,
        HttpMethod httpMethod,
        string route,
        Delegate handler)
    {
        if (httpMethod == HttpMethod.Get)
        {
            return builder.MapGet(route, handler);
        }

        if (httpMethod == HttpMethod.Post)
        {
            return builder.MapPost(route, handler);
        }

        if (httpMethod == HttpMethod.Patch)
        {
            return builder.MapPatch(route, handler);
        }

        if (httpMethod == HttpMethod.Delete)
        {
            return builder.MapDelete(route, handler);
        }

        throw new NotSupportedException($"HTTP method {httpMethod} is not supported.");
    }
}
