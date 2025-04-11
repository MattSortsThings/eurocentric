using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Features.Shared.Security;

internal static class AuthorizationPolicies
{
    internal static Action<AuthorizationPolicyBuilder> RequireAuthenticatedUser =
        static builder => builder.RequireAuthenticatedUser();
}
