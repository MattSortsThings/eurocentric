using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Eurocentric.Features.Shared.EndpointMapping;

internal static class RouteHandlerBuilderExtensions
{
    internal static RouteHandlerBuilder ProducesProblems(this RouteHandlerBuilder builder, IEnumerable<int> statusCodes)
    {
        foreach (int statusCode in statusCodes)
        {
            builder.ProducesProblem(statusCode);
        }

        return builder;
    }
}
