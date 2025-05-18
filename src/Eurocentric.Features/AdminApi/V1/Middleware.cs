using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1;

/// <summary>
///     Extension methods to be invoked when configuring HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    private const string EndpointGroupName = "AdminApi.V1";

    /// <summary>
    ///     Maps the Admin API V1 endpoints.
    /// </summary>
    /// <param name="adminApiGroup">The Admin API endpoint group.</param>
    internal static void MapAdminApiV1Endpoints(this IEndpointRouteBuilder adminApiGroup)
    {
        RouteGroupBuilder v1Group = adminApiGroup.MapGroup("v{version:apiVersion}")
            .WithGroupName(EndpointGroupName)
            .RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v1Group.MapGetContest();

        v1Group.MapGetCountry()
            .MapGetCountries()
            .MapCreateCountry();
    }
}
