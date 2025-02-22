using Eurocentric.Shared.Security;
using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.AdminApi.Common;

internal sealed class AdminApiAuthorizationPolicy : IApiAuthorizationPolicy
{
    public void Configure(AuthorizationPolicyBuilder policyBuilder) =>
        policyBuilder.RequireAuthenticatedUser().RequireRole(Roles.Administrator);
}
