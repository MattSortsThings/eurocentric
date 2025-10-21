using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.Security;

internal sealed class ConfigureApiKeysOptions(IConfiguration configuration) : IConfigureOptions<ApiKeysOptions>
{
    private const string AppSettingsKey = "ApiKeys";

    public void Configure(ApiKeysOptions options) => configuration.Bind(AppSettingsKey, options);
}
