using Eurocentric.Apis.Admin.V0;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Admin;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned Admin API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UseAdminApiVersionedEndpoints(this WebApplication app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("AdminApi").MapGroup("admin/api");

        apiGroup.MapV0EndpointGroup();
    }
}
