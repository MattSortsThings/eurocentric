using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.DataAccess.Common;

/// <summary>
///     Configures the database connection options.
/// </summary>
/// <param name="configuration">Contains configuration properties for the application.</param>
internal sealed class ConfigureDbConnectionOptions(IConfiguration configuration)
    : IConfigureOptions<DbConnectionOptions>
{
    private const string AppSettingsKey = "DbConnection";

    public void Configure(DbConnectionOptions options) => configuration.Bind(AppSettingsKey, options);
}
