using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Features.Shared.Security;

internal sealed class ConfigureApiKeysOptions(IConfiguration configuration) : IConfigureOptions<ApiKeysOptions>
{
    public void Configure(ApiKeysOptions options) => configuration.GetSection("ApiKeys").Bind(options);
}
