using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Eurocentric.Components.Security;

/// <summary>
///     Extension methods for the <see cref="RouteGroupBuilder" /> class.
/// </summary>
public static class RouteGroupBuilderExtensions
{
    /// <summary>
    ///     Adds an authorization policy for the endpoint group: the client must be authenticated.
    /// </summary>
    /// <param name="builder">The endpoint route group builder.</param>
    /// <returns>The same <see cref="RouteGroupBuilder" /> instance, so that method invocations can be chained.</returns>
    public static RouteGroupBuilder RequiresAuthenticatedClient(this RouteGroupBuilder builder)
    {
        builder.RequireAuthorization(nameof(AuthorizationPolicies.Authenticated));

        return builder;
    }

    /// <summary>
    ///     Adds an authorization policy for the endpoint group: the client must have the "Administrator" role.
    /// </summary>
    /// <param name="builder">The endpoint route group builder.</param>
    /// <returns>The same <see cref="RouteGroupBuilder" /> instance, so that method invocations can be chained.</returns>
    public static RouteGroupBuilder RequiresAdministratorRole(this RouteGroupBuilder builder)
    {
        builder.RequireAuthorization(nameof(AuthorizationPolicies.AdministratorRole));

        return builder;
    }

    /// <summary>
    ///     Adds an authorization policy for the endpoint group: the client must have the "User" role.
    /// </summary>
    /// <param name="builder">The endpoint route group builder.</param>
    /// <returns>The same <see cref="RouteGroupBuilder" /> instance, so that method invocations can be chained.</returns>
    public static RouteGroupBuilder RequiresUserRole(this RouteGroupBuilder builder)
    {
        builder.RequireAuthorization(nameof(AuthorizationPolicies.UserRole));

        return builder;
    }
}
