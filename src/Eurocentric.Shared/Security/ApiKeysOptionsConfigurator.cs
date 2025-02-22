using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Shared.Security;

internal sealed class ApiKeysOptionsConfigurator(IConfiguration configuration) : IConfigureOptions<ApiKeysOptions>
{
    public void Configure(ApiKeysOptions options) => configuration.GetSection("ApiKeys").Bind(options);
}
