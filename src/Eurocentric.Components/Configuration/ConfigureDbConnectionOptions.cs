using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eurocentric.Components.Configuration;

/// <summary>
///     Configures options used for connecting to the application database.
/// </summary>
/// <param name="configuration">Contains configuration properties for the application.</param>
public sealed class ConfigureDbConnectionOptions(IConfiguration configuration) : IConfigureOptions<DbConnectionOptions>
{
    /// <inheritdoc />
    public void Configure(DbConnectionOptions options) =>
        configuration.GetSection(DbConnectionOptions.AppSettingsKey).Bind(options);
}
