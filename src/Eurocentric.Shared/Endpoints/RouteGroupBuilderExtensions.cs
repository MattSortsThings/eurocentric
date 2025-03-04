using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Shared.Endpoints;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder ProducesProblems(this RouteGroupBuilder builder, IEnumerable<int> statusCodes)
    {
        foreach (int statusCode in statusCodes)
        {
            builder.ProducesProblem(statusCode);
        }

        return builder;
    }
}
