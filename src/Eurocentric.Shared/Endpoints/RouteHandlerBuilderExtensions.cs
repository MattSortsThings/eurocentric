using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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

    public static RouteHandlerBuilder ProducesProblems(this RouteHandlerBuilder builder, IEnumerable<int> statusCodes)
    {
        foreach (int statusCode in statusCodes)
        {
            builder.ProducesProblem(statusCode);
        }

        return builder;
    }
}
