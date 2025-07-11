using Eurocentric.Features.AdminApi.V1.Common.Constants;
using Eurocentric.Features.AdminApi.V1.Contests;
using Eurocentric.Features.AdminApi.V1.Countries;
using Eurocentric.Features.Shared.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Features.AdminApi.V1;

/// <summary>
///     Extension methods to be invoked when configuring the web application HTTP request pipeline middleware.
/// </summary>
internal static class Middleware
{
    /// <summary>
    ///     Maps the Admin API version 1 endpoints to the Admin API endpoint group.
    /// </summary>
    /// <param name="apiGroup">The Admin API endpoint group.</param>
    internal static void MapAdminApiV1Endpoints(this IEndpointRouteBuilder apiGroup)
    {
        RouteGroupBuilder v1Group = apiGroup.MapGroup("v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole))
            .WithGroupName(EndpointConstants.GroupName)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        v1Group.MapGetContest();

        v1Group.MapCreateCountry()
            .MapGetCountry()
            .MapGetCountries();
    }
}
