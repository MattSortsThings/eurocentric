using Eurocentric.Features.PublicApi.V0.Common.Constants;
using Eurocentric.Features.PublicApi.V0.Queryables;
using Eurocentric.Features.PublicApi.V0.Rankings;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.PublicApi.V0;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Adds the Public API v0.x endpoints.
    /// </summary>
    /// <param name="builder">The endpoint route builder to which the endpoints are to be added.</param>
    internal static void MapV0Endpoints(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder v0Group = builder.MapGroup("v{version:apiVersion}")
            .WithGroupName(EndpointNames.Group)
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClient)
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        v0Group.MapQueryablesEndpoints();

        v0Group.MapRankingsEndpoints();
    }
}
