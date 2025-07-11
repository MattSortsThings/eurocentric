using Eurocentric.Features.AdminApi.V1.Common.Constants;
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
    ///     Maps the Admin API version 1 endpoints to the web application.
    /// </summary>
    /// <param name="app">The web application.</param>
    internal static void MapAdminApiV1Endpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi(ApiNames.Id)
            .MapGroup("admin/api/v{apiVersion:apiVersion}")
            .RequireAuthorization(nameof(AuthorizationPolicies.RequireAuthenticatedClientWithAdministratorRole))
            .WithGroupName(ApiNames.EndpointGroup)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden);

        apiGroup.MapCreateCountry()
            .MapGetCountry()
            .MapGetCountries();
    }
}
