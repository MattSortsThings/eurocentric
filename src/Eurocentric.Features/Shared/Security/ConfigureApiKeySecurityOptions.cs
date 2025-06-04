using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Features.Shared.Security;

internal sealed class ConfigureApiKeySecurityOptions(IConfiguration configuration) : IConfigureOptions<ApiKeySecurityOptions>
{
    public void Configure(ApiKeySecurityOptions options) => configuration.GetSection("ApiKeySecurity").Bind(options);
}
