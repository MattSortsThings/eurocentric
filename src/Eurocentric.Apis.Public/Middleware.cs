using Eurocentric.Apis.Public.V0;
using Eurocentric.Apis.Public.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Apis.Public;

/// <summary>
///     Extension methods to be invoked when configuring web application middleware.
/// </summary>
public static class Middleware
{
    /// <summary>
    ///     Configures the web application to use the versioned Public API endpoints.
    /// </summary>
    /// <param name="app">The web application.</param>
    public static void UsePublicApiVersionedEndpoints(this WebApplication app)
    {
        RouteGroupBuilder apiGroup = app.NewVersionedApi("PublicApi").MapGroup("public/api");

        apiGroup.MapV0EndpointGroup();
        apiGroup.MapV1EndpointGroup();
    }
}
