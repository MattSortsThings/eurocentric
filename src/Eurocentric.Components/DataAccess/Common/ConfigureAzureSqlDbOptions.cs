using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Common;

internal sealed class ConfigureAzureSqlDbOptions(IConfiguration configuration) : IConfigureOptions<AzureSqlDbOptions>
{
    private const string AppSettingsKey = "AzureSqlDb";

    public void Configure(AzureSqlDbOptions options) => configuration.Bind(AppSettingsKey, options);
}
