using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Shared.Security;

internal sealed class ApiKeysConfigurator(IConfiguration configuration) : IConfigureOptions<ApiKeysOptions>
{
    public void Configure(ApiKeysOptions options) => configuration.GetSection("ApiKeys").Bind(options);
}
