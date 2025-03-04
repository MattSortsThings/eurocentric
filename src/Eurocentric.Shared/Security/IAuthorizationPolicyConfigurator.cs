using Microsoft.AspNetCore.Authorization;

namespace Eurocentric.Shared.Security;

public interface IAuthorizationPolicyConfigurator
{
    public string PolicyName { get; }

    public void ConfigurePolicy(AuthorizationPolicyBuilder builder);
}
