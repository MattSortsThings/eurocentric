using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Shared.Security;

public static class Policies
{
    public static class AuthenticatedAdminOnly
    {
        internal static readonly Action<AuthorizationPolicyBuilder> ConfigurePolicy = policy =>
            policy.RequireAuthenticatedUser()
                .RequireRole(Roles.Admin);
    }

    public static class AuthenticatedAdminOrUserOnly
    {
        internal static readonly Action<AuthorizationPolicyBuilder> ConfigurePolicy = policy =>
            policy.RequireAuthenticatedUser()
                .RequireRole(Roles.Admin, Roles.User);
    }
}
