using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.PublicApi.Common;

internal sealed class AuthorizationPolicyConfigurator : IAuthorizationPolicyConfigurator
{
    internal const string Name = "PublicApiAuthorizationPolicy";

    public string PolicyName => Name;

    public void ConfigurePolicy(AuthorizationPolicyBuilder builder) =>
        builder.RequireAuthenticatedUser()
            .RequireRole(SecurityConstants.Roles.User);
}
