using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Security;

/// <summary>
///     Extension methods for the <see cref="RouteGroupBuilder" /> class.
/// </summary>
public static class RouteGroupBuilderExtensions
{
    /// <summary>
    ///     Sets the authorization policy for the endpoint group.
    /// </summary>
    /// <param name="builder">The endpoint route group builder.</param>
    /// <returns>The same <see cref="RouteGroupBuilder" /> instance, so that method invocations can be chained.</returns>
    public static RouteGroupBuilder RequiresAuthenticatedClient(this RouteGroupBuilder builder)
    {
        builder.RequireAuthorization(AuthorizationPolicies.RequireAuthenticatedClient);

        return builder;
    }
}
