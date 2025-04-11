using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Features.Shared.Security;

internal static class AuthorizationPolicies
{
    internal static Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser =>
        static builder => builder.RequireAuthenticatedUser();

    internal static Action<AuthorizationPolicyBuilder> RequireAuthenticatedClientWithUserRole =>
        static builder => builder.RequireAuthenticatedUser().RequireRole(Roles.User);
}
