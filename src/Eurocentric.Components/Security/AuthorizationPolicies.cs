using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Components.Security;

internal static class AuthorizationPolicies
{
    internal static Action<AuthorizationPolicyBuilder> Authenticated => builder => builder.RequireAuthenticatedUser();

    internal static Action<AuthorizationPolicyBuilder> AdministratorRole =>
        static builder => builder.RequireRole(Roles.Administrator);

    internal static Action<AuthorizationPolicyBuilder> UserRole => static builder => builder.RequireRole(Roles.User);
}
