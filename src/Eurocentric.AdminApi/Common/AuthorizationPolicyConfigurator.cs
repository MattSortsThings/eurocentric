using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.AdminApi.Common;

internal sealed class AuthorizationPolicyConfigurator : IAuthorizationPolicyConfigurator
{
    internal const string Name = "AdminApiAuthorizationPolicy";

    public string PolicyName => Name;

    public void ConfigurePolicy(AuthorizationPolicyBuilder builder) =>
        builder.RequireAuthenticatedUser()
            .RequireRole(SecurityConstants.Roles.Admin);
}
